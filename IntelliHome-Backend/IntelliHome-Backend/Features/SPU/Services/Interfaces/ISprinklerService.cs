using Data.Models.SPU;

namespace IntelliHome_Backend.Features.SPU.Services.Interfaces
{
    public interface ISprinklerService
    {
        Task<Sprinkler> CreateSprinkler(Sprinkler sprinkler);
    }
}
