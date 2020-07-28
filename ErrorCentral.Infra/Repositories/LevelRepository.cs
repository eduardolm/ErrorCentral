using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;

namespace ErrorCentral.Infra.Repositories
{
    public class LevelRepository : GenericRepository<Level>, ILevelRepository
    {
        public LevelRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}