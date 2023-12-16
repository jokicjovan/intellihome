using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Shared.DTOs
{
    public class SmartDeviceDTO
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String Category { get; set; }
        public String Type { get; set; }
        public Boolean IsConnected { get; set; }
        public Boolean IsOn { get; set; }
        public Guid SmartHomeId { get; set; }


        public SmartDeviceDTO()
        {
        }

        public SmartDeviceDTO(SmartDevice smartDevice)
        {
            Id = smartDevice.Id;
            Name = smartDevice.Name;
            Category = smartDevice.Category.ToString();
            Type = smartDevice.Type.ToString();
            IsConnected = smartDevice.IsConnected;
            IsOn = smartDevice.IsOn;
        }
    }
}
