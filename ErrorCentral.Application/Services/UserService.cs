using System.Linq;
using System.Net.Http;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Interfaces;
using FluentValidation;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace ErrorCentral.Application.Services
{
    public class UserService : GenericService<User>, IUserService
    {
        private IConfiguration Configuration { get; }
        public UserService(IGenericRepository<User> repository, IValidator<User> validator) : base(repository, validator)
        {
        }
        
        public UserService(IGenericRepository<User> repository, IValidator<User> validator, IConfiguration configuration) : base(repository, validator)
        {
            Configuration = configuration;
        }

        public TokenResponse Login(User user)
        {
            var tokenEnvironmentVars = Configuration.GetChildren().ToList();
            var client = new HttpClient();
            var response = client.RequestPasswordTokenAsync(new PasswordTokenRequest
            {
                Address = "https://localhost:5001/connect/token",
                GrantType = tokenEnvironmentVars.FirstOrDefault(x => x.Key == "GrantType")?.Value,
                ClientId = tokenEnvironmentVars.FirstOrDefault(x => x.Key == "ClientId")?.Value,
                ClientSecret = tokenEnvironmentVars.FirstOrDefault(x => x.Key == "ClientSecret")?.Value,
                Scope = tokenEnvironmentVars.FirstOrDefault(x => x.Key == "Scope")?.Value,
                UserName = user.Email,
                Password = user.Password
            }).Result;
            return response;
        }
        
    }
}