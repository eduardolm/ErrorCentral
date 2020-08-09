using ErrorCentral.Domain.Models;
using IdentityModel.Client;

namespace ErrorCentral.Application.Interfaces
{
    public interface IUserService : IGenericService<User>
    {
        TokenResponse Login(User user);
    }
}