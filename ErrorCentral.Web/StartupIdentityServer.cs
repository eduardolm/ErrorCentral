using System;
using ErrorCentral.Application.Services;
using ErrorCentral.Infra.Context;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ErrorCentral.Web
{
    public class StartupIdentityServer
    {
        private IWebHostEnvironment Environment { get; }

        public StartupIdentityServer(IWebHostEnvironment environment)
        {
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MainContext>();
            services.AddScoped<IResourceOwnerPasswordValidator, PasswordValidatorService>();
            services.AddScoped<IProfileService, UserProfileService>();
            
            var builder = services.AddIdentityServer()
                .AddInMemoryPersistedGrants()
                .AddInMemoryIdentityResources(IdentityConfig.GetIdentityResources())
                .AddInMemoryApiResources(IdentityConfig.GetApis())
                .AddInMemoryClients(IdentityConfig.GetClients())
                .AddInMemoryApiScopes(IdentityConfig.GetApiScopes())
                .AddProfileService<UserProfileService>();
            
            
            if (Environment.IsDevelopment())
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                throw new Exception("Production environment needs real key.");
            }
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {                        
            app.UseIdentityServer();            
        }
    }
}