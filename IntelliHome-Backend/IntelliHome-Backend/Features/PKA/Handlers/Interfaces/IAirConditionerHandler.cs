using Data.Models.PKA;
using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;

namespace IntelliHome_Backend.Features.PKA.Handlers.Interfaces
{
    public interface IAirConditionerHandler : ISmartDeviceHandler
    {
        void AddSchedule(AirConditioner airConditioner, string timestamp, string mode, double temperature);
        void ChangeMode(AirConditioner airConditioner, string mode);
        void ChangeTemperature(AirConditioner airConditioner, double brightness);
    }
}
