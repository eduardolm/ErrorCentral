using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;

namespace ErrorCentral.Application.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        public UserService(IGenericRepository<User> repository, IValidator<User> validator) : base(repository, validator)
        {
        }
    }
}