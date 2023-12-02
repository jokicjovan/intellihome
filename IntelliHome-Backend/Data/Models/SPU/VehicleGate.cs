using Data.Models.Shared;

namespace Data.Models.SPU
{
    public class VehicleGate : SmartDevice
    {
        public Boolean IsPublic { get; set; }
        public String? CurrentLicencePlate { get; set; }
        public List<String> AllowedLicencePlates { get; set; }
        public Double PowerPerHour { get; set; }

        public VehicleGate()
        {
            AllowedLicencePlates = new List<String>();
        }

    }
}
