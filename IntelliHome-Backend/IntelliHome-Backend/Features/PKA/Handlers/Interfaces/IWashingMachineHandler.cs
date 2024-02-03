using Data.Models.PKA;
using Data.Models.Shared;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Handlers.Interfaces
{
    public interface IWashingMachineHandler : ISmartDeviceHandler
    {
        void AddSchedule(WashingMachine washingMachine, string timestamp, string mode, double temperature);
        void ChangeMode(WashingMachine washingMachine, string mode,double temperature);
        Task<bool> ConnectToSmartDevice(SmartDevice smartDevice);
    }
}
