using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;

namespace ErrorCentral.Infra.Repositories
{
    public class StatusRepository : GenericRepository<Status>, IStatusRepository
    {
        public StatusRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}