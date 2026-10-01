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
    }
}