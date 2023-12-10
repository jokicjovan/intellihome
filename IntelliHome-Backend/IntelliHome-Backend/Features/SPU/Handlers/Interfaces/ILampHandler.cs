using Data.Models.SPU;
using IntelliHome_Backend.Features.Shared.Handlers.Interfaces;

namespace IntelliHome_Backend.Features.SPU.Handlers.Interfaces
{
    public interface ILampHandler : ISmartDeviceHandler
    {
        void ChangeMode(Lamp lamp, bool isAuto);
    }
}
