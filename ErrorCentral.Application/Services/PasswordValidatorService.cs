using System.Linq;
using System.Threading.Tasks;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;

namespace ErrorCentral.Application.Services
{
    public class PasswordValidatorService : IResourceOwnerPasswordValidator
    {
        private readonly MainContext _dbContext;

        public PasswordValidatorService(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            User user = null;

            user =
                (from u in _dbContext.User
                    where u.Password.Equals(context.Password)
                    where u.Email.Equals(context.UserName)
                    select u).FirstOrDefault();
            
            if (user != null)
            {
                context.Result = new GrantValidationResult(
                    user.Id.ToString(), OidcConstants.AuthenticationMethods.Password);
            }
            else
            {
                context.Result = new GrantValidationResult(
                    TokenRequestErrors.InvalidGrant, "Invalid username or password.");
            }

            return Task.CompletedTask;
        }
    }
}