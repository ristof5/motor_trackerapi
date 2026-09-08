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

app.Run();