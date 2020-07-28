using System.Collections.Generic;
using System.Net;
using AutoMapper;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Application.Services;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Infra;
using ErrorCentral.Web;
using ErrorCentral.Web.Controllers;
using IdentityModel.Client;
using Newtonsoft.Json;
using Xunit;

namespace ErrorCentral.Test.Unit.Web.Controllers
{
    public class UserControllerTest : UserAuthenticationTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Should_Get_User_By_Id(int userId)
        {
            var fakeContext = new FakeContext("ControllerGetUserById");
            fakeContext.FillWith<User>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new UserController(service, mapper);
                var user = service.GetById(userId);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"/user/{userId}").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrUser = JsonConvert.DeserializeObject<User>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(user.Id, retrUser.Id);
                Assert.Equal(user.FullName, retrUser.FullName);
            }
        }
        
        [Fact]
        public void Should_Get_All_Users()
        {
            var fakeContext = new FakeContext("ControllerGetAll");
            fakeContext.FillWith<User>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new UserController(service, mapper);
                var user = service.GetById(1);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"/user").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrUser = JsonConvert.DeserializeObject<List<User>>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<List<User>>(retrUser);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.NotEmpty(retrUser);
                foreach (var testuser in retrUser)
                {
                    Assert.IsType<User>(testuser);
                    Assert.NotEqual(0, testuser.Id);
                    Assert.IsType<string>(testuser.FullName);
                    Assert.IsType<string>(testuser.Email);
                }
            }
        }
    }
}