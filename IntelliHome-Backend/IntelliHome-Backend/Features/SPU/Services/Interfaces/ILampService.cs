using Data.Models.SPU;

namespace IntelliHome_Backend.Features.SPU.Services.Interfaces
{
    public interface ILampService
    {
        Task<Lamp> CreateLamp(Lamp lamp);
    }
}
