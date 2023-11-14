using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Services.Interfaces
{
    public interface IDeviceConnectionService
    {
        public Task<bool> ConnectWithSmartDevice(SmartDevice smartDevice);
    }
}
