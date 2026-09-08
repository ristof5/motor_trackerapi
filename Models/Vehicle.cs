namespace MotorTracker.Api.Models
{
    public class Vehicle
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; // misal: "Vario 150"
        public string Brand { get; set; } = string.Empty; // misal: "Honda"
        public int CurrentMileage { get; set; } // Kilometer terakhir
    }
}