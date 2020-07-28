using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;

namespace ErrorCentral.Infra.Repositories
{
    public class EnvironmentRepository : GenericRepository<Environment>, IEnvironmentRepository
    {
        public EnvironmentRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}