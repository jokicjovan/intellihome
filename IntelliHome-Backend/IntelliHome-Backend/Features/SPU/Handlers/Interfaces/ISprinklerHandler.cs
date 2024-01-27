using Data.Models.SPU;
using IntelliHome_Backend.Features.Home.Handlers.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Handlers.Interfaces
{
    public interface ISprinklerHandler : ISmartDeviceHandler
    {
        void AddSchedule(Sprinkler sprinkler, string timestamp, bool set_spraying);
        void SetSpraying(Sprinkler sprinkler, bool isSpraying);
    }
}
