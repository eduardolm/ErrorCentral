using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;

namespace ErrorCentral.Infra.Repositories
{
    public class LayerRepository : GenericRepository<Layer>, ILayerRepository
    {
        public LayerRepository(MainContext dbContext) : base(dbContext)
        {
        }
    }
}