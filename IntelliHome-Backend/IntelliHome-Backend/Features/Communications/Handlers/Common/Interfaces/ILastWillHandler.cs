using Data.Models.Shared;

namespace IntelliHome_Backend.Features.Communications.Handlers.Common.Interfaces
{
    public interface ILastWillHandler
    {
        Task SetupLastWillHandler();
    }
}
