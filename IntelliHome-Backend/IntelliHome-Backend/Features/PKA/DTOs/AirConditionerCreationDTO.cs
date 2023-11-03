using IntelliHome_Backend.Features.Shared.DTOs;

namespace IntelliHome_Backend.Features.PKA.DTOs
{
    public class AirConditionerCreationDTO : SmartDeviceDTO
    {
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public double CurrentTemperature { get; set; }

        public AirConditionerCreationDTO(string name, string image, double minTemperature, double maxTemperature, double currentTemperature, double powerPerHour)
        {
            Name = name;
            Image = image;
            MinTemperature = minTemperature;
            MaxTemperature = maxTemperature;
            CurrentTemperature = currentTemperature;
            PowerPerHour = powerPerHour;
        }
    }
}
