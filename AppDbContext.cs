using Microsoft.EntityFrameworkCore;
using MotorTracker.Api.Models;

namespace MotorTracker.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // Ketiga baris ini WAJIB ada, karena melambangkan 3 tabel di MySQL
        public DbSet<Vehicle> Vehicles => Set<Vehicle>();
        public DbSet<MaintenanceLog> MaintenanceLogs => Set<MaintenanceLog>();
        public DbSet<ServiceConfig> ServiceConfigs => Set<ServiceConfig>(); 

        protected override void onModelCreating(ModelBuilder modelBuilder)
        {
            base.onModelCreating(modelBuilder);

            // seed data atau data awal

            modelBuilder.Entity<ServiceConfig>().HasData(
                new ServiceConfig { Id = 1, PartName = "Oli Mesin", IntervalKm = 2000, Description = "Penggantian oli mesin"},
                new ServiceConfig { Id = 2, PartName = "Oli Gardan", IntervalKm = 8000, Description = "Penggantian oli gardan"},
                new ServiceConfig { Id = 3, PartName = "Filter Oli", IntervalKm = 4000, Description = "Penggantian filter oli"},
                new ServiceConfig { Id = 4, PartName = "Air Filter", IntervalKm = 8000, Description = "Penggantian air filter"},
                new ServiceConfig { Id = 5, PartName = "Busi", IntervalKm = 10000, Description = "Penggantian busi"},
                new ServiceConfig { Id = 6, PartName = "Kampas Rem Depan", IntervalKm = 10000, Description = "Penggantian kampas rem depan"},
                new ServiceConfig { Id = 7, PartName = "Kampas Rem Belakang", IntervalKm = 10000, Description = "Penggantian kampas rem belakang"},
                new ServiceConfig { Id = 8, PartName = "V-Belt", IntervalKm = 24000, Description = "Penggantian v-belt"},
                new ServiceConfig { Id = 9, PartName = "Roller", IntervalKm = 24000, Description = "Penggantian roller"}
            );  
        }
    }
}