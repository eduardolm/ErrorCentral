using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;
using ErrorCentral.Application.Interfaces;
using ErrorCentral.Application.Services;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Interfaces;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Web.Controllers;
using IdentityServer4.Services;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using AutoMapper;
using ErrorCentral.Domain.Models;
using FluentValidation;
using ErrorCentral.Application.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Logging;
using ErrorCentral.Web;
using ErrorCentral.Web.Interfaces;

namespace ErrorCentral.Web
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public StartupIdentityServer IdentityServerStartup { get; }
        
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            if (!environment.IsEnvironment("Testing"))
                IdentityServerStartup = new StartupIdentityServer(environment);
        }
        public void ConfigureServices(IServiceCollection services)
        {
            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

            services.AddMvcCore(options => options.EnableEndpointRouting = false)
                .AddAuthorization(opt =>
                {
                    opt.AddPolicy("codenation", policy =>
                    {
                        policy.RequireAuthenticatedUser();
                    });
                    opt.AddPolicy("IsAdmin", policy => policy.RequireClaim("admin"));
                    opt.AddPolicy("IsUser", policy => policy.RequireClaim("user"));
                })
                .AddNewtonsoftJson(options => 
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddDbContext<MainContext>(opt => opt
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ILogService, LogService>();
            services.AddScoped<ILayerService, LayerService>();
            services.AddScoped<ILevelService, LevelService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IEnvironmentService, EnvironmentService>();
            services.AddTransient<IValidator<User>, UserValidator>();
            services.AddTransient<IValidator<Log>, LogValidator>();
            services.AddTransient<IValidator<Layer>, LayerValidator>();
            services.AddTransient<IValidator<Level>, LevelValidator>();
            services.AddTransient<IValidator<Status>, StatusValidator>();
            services.AddTransient<IValidator<Environment>, EnvironmentValidator>();
            services.AddScoped<IResourceOwnerPasswordValidator, PasswordValidatorService>();
            services.AddScoped<IProfileService, UserProfileService>();
            services.AddAutoMapper(typeof(Startup));
            services.AddScoped(typeof(IGenericService<>), typeof(GenericService<>));
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IGenericController<>), typeof(GenericController<>));
            
            services.AddControllers();
            
            if (IdentityServerStartup != null)
                IdentityServerStartup.ConfigureServices(services);
            
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:5001";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });

            services.AddCors(opt => opt
                .AddPolicy("MyPolicy", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            IdentityServerStartup?.Configure(app, env);

            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope
                    .ServiceProvider
                    .GetRequiredService<MainContext>();
            
                context
                    .Database
                    .Migrate();
            }
            IdentityModelEventSource.ShowPII = true;

            app.UseDefaultFiles();
            app.UseStaticFiles()
                .UseDefaultFiles(new DefaultFilesOptions
                {
                    DefaultFileNames = new List<string> {"index.html"}
                });

            app.UseCors("MyPolicy");
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}