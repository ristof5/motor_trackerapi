using Microsoft.EntityFrameworkCore;
using MotorTracker.Api;
using MotorTracker.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// Mendaftarkan AppDbContext ke sistem .NET dan mengarahkannya ke MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

// ==============================================================
// BLOK API UNTUK KENDARAAN (VEHICLES)
// ==============================================================

// 1. GET: Mengambil semua data motor dari MySQL
app.MapGet("/api/vehicles", async (AppDbContext db) =>
{
    return await db.Vehicles.ToListAsync();
});

// 2. GET by ID: Mengambil spesifik 1 motor
app.MapGet("/api/vehicles/{id}", async (int id, AppDbContext db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);
    return vehicle is null ? Results.NotFound("Motor tidak ditemukan") : Results.Ok(vehicle);
});

// 3. POST: Menyimpan motor baru ke MySQL
app.MapPost("/api/vehicles", async (Vehicle vehicle, AppDbContext db) =>
{
    db.Vehicles.Add(vehicle);
    await db.SaveChangesAsync(); // Simpan ke database
    return Results.Created($"/api/vehicles/{vehicle.Id}", vehicle);
});

// 4. PUT: Memperbarui data motor (misalnya update Kilometer)
app.MapPut("/api/vehicles/{id}", async (int id, Vehicle inputVehicle, AppDbContext db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);
    if (vehicle is null) return Results.NotFound();

    vehicle.Name = inputVehicle.Name;
    vehicle.Brand = inputVehicle.Brand;
    vehicle.CurrentMileage = inputVehicle.CurrentMileage;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

// 5. DELETE: Menghapus data motor
app.MapDelete("/api/vehicles/{id}", async (int id, AppDbContext db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);
    if (vehicle is null) return Results.NotFound();

    db.Vehicles.Remove(vehicle);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

// ==============================================================
// BLOK API UNTUK SERVICE HISTORY
// ==============================================================
// FITUR PREDIKSI DINAMIS (Multi-Sparepart)
app.MapGet("/api/vehicles/{id}/reminder", async (int id, AppDbContext db) =>
{
    var vehicle = await db.Vehicles.FindAsync(id);
    if (vehicle is null) return Results.NotFound("Motor tidak ditemukan");

    // 1. Ambil SEMUA jenis konfigurasi servis dari database
    var configs = await db.ServiceConfigs.ToListAsync();
    var report = new List<object>(); // Keranjang untuk menyimpan hasil laporan

    // 2. Looping (Cek satu per satu setiap sparepart)
    foreach (var config in configs)
    {
        // Cari riwayat servis terakhir berdasarkan nama part-nya (misal mencari kata "Oli")
        var lastService = await db.MaintenanceLogs
            .Where(l => l.VehicleId == id && l.ServiceType.Contains(config.PartName))
            .OrderByDescending(l => l.ServiceDate)
            .FirstOrDefaultAsync();

        if (lastService is null)
        {
            report.Add(new { Part = config.PartName, Status = "Belum Ada Riwayat" });
            continue;
        }

        // 3. Kalkulasi Dinamis (Memakai config.IntervalKm dari database, bukan angka 2000 hardcode lagi!)
        int targetKm = lastService.MileageAtService + config.IntervalKm;
        int remainingKm = targetKm - vehicle.CurrentMileage;

        string statusText = remainingKm <= 0 ? "BAHAYA: Segera Ganti!" : 
                            (remainingKm <= 200 ? "PERINGATAN: Sudah Dekat" : "AMAN");

        report.Add(new {
            Part = config.PartName,
            Status = statusText,
            RemainingMileage = remainingKm,
            TargetNextService = targetKm
        });
    }

    return Results.Ok(new {
        VehicleName = vehicle.Name,
        CurrentMileage = vehicle.CurrentMileage,
        HealthReport = report // Menampilkan laporan semua sparepart sekaligus!
    });
});

app.Run();