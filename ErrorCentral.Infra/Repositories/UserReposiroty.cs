using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;

namespace ErrorCentral.Infra.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(MainContext dbContext) : base(dbContext)
        {
        }

    }
}