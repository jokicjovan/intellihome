using Data.Context;
using Data.Models.PKA;
using IntelliHome_Backend.Features.PKA.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;

namespace IntelliHome_Backend.Features.PKA.Repositories
{
    public class WashingMachineModeRepository : CrudRepository<WashingMachineMode>, IWashingMachineModeRepository
    {
        public WashingMachineModeRepository(PostgreSqlDbContext context) : base(context) { }

        public List<WashingMachineMode> FindWashingMachineModes(List<Guid> modesIds)
        {
            return _entities.Where(e => modesIds.Contains(e.Id)).ToList();
        }

        public async Task<WashingMachineMode> FindWashingMachineModeByName(String name)
        {
            return _entities.FirstOrDefault(e => name.ToLower().Equals(e.Name.ToLower()));
        }
    }
}
