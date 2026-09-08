using System;

namespace MotorTracker.Api.Models 
{
    public class MaintainceLog
    {
        public int Id {get; set;}
        public int VehicleId {get; set;}
        public DateTime ServiceDate {get; set;}
        public int MileageService {get; set;}
        public string ServiceType {get; set; } = string.Empty;
        public decimal Cost {get; set;} 
        public string Note {get; set;} = string.Empty;
    }
}