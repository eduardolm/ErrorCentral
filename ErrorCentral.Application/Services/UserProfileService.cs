using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using IdentityServer4.Models;
using IdentityServer4.Services;
using IdentityServer4.Validation;

namespace ErrorCentral.Application.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly MainContext _dbContext;

        public UserProfileService(MainContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            if (!(context.ValidatedRequest is ValidatedTokenRequest request)) return Task.CompletedTask;
            var user =
                (from u in _dbContext.User
                    where u.Email.Equals(request.UserName)
                    select u).FirstOrDefault();

            context.IssuedClaims.AddRange(GetUserClaims(user).ToList());

            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }

        private static IEnumerable<Claim> GetUserClaims(User user)
        {
            var role = string.Empty;
            role = "user";

            return new[]
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, role)
            };
        }
    }
}