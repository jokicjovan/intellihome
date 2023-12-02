using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Shared.Handlers.Interfaces
{
    public interface ILastWillHandler
    {
        Task SetupLastWillHandler();
    }
}
