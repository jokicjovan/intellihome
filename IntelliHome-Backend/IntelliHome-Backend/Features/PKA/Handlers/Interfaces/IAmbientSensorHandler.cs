using Data.Models.PKA;

namespace IntelliHome_Backend.Features.PKA.Handlers.Interfaces
{
    public interface IAmbientSensorHandler
    {
        void SubscribeToAmbientSensorsFromDatabase();
        void SubscribeToAmbientSensor(string topic);
        void PublishMessageToAmbientSensor(AmbientSensor ambientSensor, string payload);
        Task<bool> AddAmbientSensorToSimulator(AmbientSensor ambientSensor);
    }
}
