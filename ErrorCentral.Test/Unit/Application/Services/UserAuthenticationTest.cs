using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using AutoMapper;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Web;
using ErrorCentral.Web.Controllers;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using ErrorCentral.Tests.Unit.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace ErrorCentral.Test.Unit.Application.Services
{
    public class UserAuthenticationTest
    {
        protected TestServer server;
        protected TestServer authServer;

        public static IConfiguration InitConfiguration()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.test.json")
                .Build();
            return config;
        }
        
        public UserAuthenticationTest()
        {
            var authBuilder = new WebHostBuilder().
                UseEnvironment("Development").
                UseStartup<StartupIdentityServer>();
            authServer = new TestServer(authBuilder);            
            authServer.BaseAddress = new Uri("https://localhost:5001");

            var builder = new WebHostBuilder().
                UseEnvironment("Testing").
                ConfigureServices(services => {
                    services.Configure<JwtBearerOptions>( "Bearer", jwtOpts => {
                        jwtOpts.BackchannelHttpHandler = authServer.CreateHandler();
                    });
                    services.AddDbContext<MainContext>(opt => opt
                        .UseSqlServer(InitConfiguration().GetConnectionString("DefaultConnection")));
                }).
                UseStartup<Startup>();

            server = new TestServer(builder);            
            server.BaseAddress = new Uri("https://localhost:5001");
        }       

        private Dictionary<string, string> GetTokenParameters(string username, string password)
        {
            var parameters = new Dictionary<string, string>();
            parameters["client_id"] = "codenation.api_client";
            parameters["client_secret"] = "codenation.api_secret";
            parameters["grant_type"] = "password";
            parameters["username"] = username;
            parameters["password"] = password;
            return parameters;
        }

        public Token GetToken(string username, string password)
        {
            var parameters = GetTokenParameters(username, password);
            var client = authServer.CreateClient();
            var response = client.PostAsync("/connect/token", 
                new FormUrlEncodedContent(parameters)).Result;
            response.EnsureSuccessStatusCode();
            return response.Content.ReadAsAsync<Token>().Result;
        }

        [Fact]
        public void Should_Has_Authorize_Attribute_On_User_Controller()
        {
            var attributes = typeof(UserController).
                GetCustomAttributes(false).
                Select(x => x.GetType().Name).
                ToList();
            Assert.Contains("AuthorizeAttribute", attributes);
        }

        [Fact]
        public void Should_All_Routes_Be_Unauthorized_When_Call_With_No_Token()
        {
            var client = server.CreateClient();
            var actual = client.GetAsync("/log").Result;            
            Assert.Equal(HttpStatusCode.Unauthorized, actual.StatusCode);
        }

        [Fact]
        public void Should_Admin_Be_Authorized_On_Route_User()
        {
            var token = GetToken("annezdz@mail.com", "chokrs3254");
            Assert.NotNull(token);

            var client = server.CreateClient();
            client.SetBearerToken(token.access_token);            

            var actual = client.GetAsync("/user/1").Result;
            Assert.NotEqual(HttpStatusCode.Unauthorized, actual.StatusCode);
            Assert.NotEqual(HttpStatusCode.Forbidden, actual.StatusCode);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        [InlineData(8)]
        [InlineData(9)]
        [InlineData(10)]
        public void Should_Generate_A_Valid_Token(int userId)
        {
            var fakeContext = new FakeContext("AuthCreateToken");
            fakeContext.FillWith<User>();
            var fakeService = fakeContext.FakeUserService().Object;

            var user = fakeService.GetById(userId);
            authServer.CreateClient();
            var actual = GetToken(user.Email, user.Password);

            Assert.NotNull(actual);
            Assert.NotEmpty(actual.access_token);
            Assert.True(actual.expires_in > 0);
            Assert.Equal("Bearer", actual.token_type);
            Assert.Equal("api1", actual.scope);
            fakeService.Dispose();
        }

        [Fact]
        public void Should_Allow_Anonymous_Create_User()
        {
            var fakeContext = new FakeContext("AllowAnonymousCreateUser");
            fakeContext.FillWith<User>();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var config = InitConfiguration();
                var mapper = mockMapper.CreateMapper();
                var controller = new UserController(service, mapper, config);

                var user = new User();
                user.FullName = "Test User";
                user.Email = "test@mail.com";
                user.Password = "TestPass";
                user.CreatedAt = new DateTime(2020, 07, 25, 17, 57, 32);
                var resultA = controller.CreateNewUser(user);
                user.FullName = string.Empty;
                var resultB = controller.CreateNewUser(user);

                Assert.IsAssignableFrom<ActionResult>(resultA);
                Assert.IsAssignableFrom<ActionResult>(resultB);
                service.Dispose();
            }
        }
    }
}