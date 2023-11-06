using Data.Context;
using Data.Models.Home;
using IntelliHome_Backend.Features.Home.Repositories.Interfaces;
using IntelliHome_Backend.Features.Shared.Repositories;
using Microsoft.EntityFrameworkCore;

namespace IntelliHome_Backend.Features.Home.Repositories
{
    public class CityRepository : CrudRepository<City>, ICityRepository
    {
        public CityRepository(PostgreSqlDbContext context) : base(context)
        {
        }
    }
}
