using Data.Models.Home;
using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Home.Handlers.Interfaces
{
    public interface ISmartHomeHandler
    {
        void PublishMessageToSmartHome(SmartHome smartHome, string payload);
        void SubscribeToSmartHome(SmartHome smartHome);
    }
}
