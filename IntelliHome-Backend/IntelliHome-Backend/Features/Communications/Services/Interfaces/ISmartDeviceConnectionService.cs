using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Services.Interfaces
{
    public interface ISmartDeviceConnectionService
    {
        Task<bool> ConnectWithSmartDevice(SmartDevice smartDevice);
        Task SetupLastWillHandler();
    }
}
