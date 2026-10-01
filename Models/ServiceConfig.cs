namespace MotorTracker.Api.Models
{
    public class ServiceConfig
    {
        public int Id { get; set; }
        public string PartName { get; set; } = string.Empty; // Contoh: "Oli Mesin", "V-Belt"
        public int IntervalKm { get; set; } // Batas ganti, misal: 2000, 24000
        public string Description { get; set; } = string.Empty;
    }
}