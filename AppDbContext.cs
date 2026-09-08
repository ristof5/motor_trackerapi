using Microsoft.EntityFrameworkCore;
using MotorTracker.Api.Models;

namespace MotorTracker.Api
{
    // class ini turunan dari DbContext
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        

        // untuk mendaftarkan table (DbSet)
        public DbSet<Vehicle>Vehicles => Set<Vehicle>();
        public DbSet<MaintainceLog> MaintainceLogs => Set<MaintainceLog>();
    }
}