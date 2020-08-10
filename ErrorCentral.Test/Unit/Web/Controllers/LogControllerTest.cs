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
using ErrorCentral.Web;
using ErrorCentral.Web.Controllers;
using IdentityModel.Client;
using Newtonsoft.Json;
using Xunit;

namespace ErrorCentral.Test.Unit.Web.Controllers
{
    public class LogControllerTest : UserAuthenticationTest
    {
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        [InlineData(3,3)]
        [InlineData(4,4)]
        [InlineData(5,5)]
        [InlineData(6,6)]
        [InlineData(7,7)]
        public void Should_Get_Log_By_Id(int userId, int logId)
        {
            var fakeContext = new FakeContext("ControllerGetLogById");
            fakeContext.FillWith<User>();
            fakeContext.FillWith<Log>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var userRepository = new UserRepository(context);
                var validator = new LogValidator();
                var userValidator = new UserValidator(context);
                var service = new LogService(repository, validator, context);
                var userService = new UserService(userRepository, userValidator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new LogController(service, mapper);
                var user = userService.GetById(userId);
                var log = service.GetById(logId);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"/log/{logId}").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrLog = JsonConvert.DeserializeObject<Log>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<Log>(retrLog);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                Assert.Equal(log.Id, retrLog.Id);
                Assert.Equal(log.Name, retrLog.Name);
                Assert.Equal(log.Description, retrLog.Description);
            }
        }
        
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        [InlineData(3,3)]
        [InlineData(4,1)]
        [InlineData(5,2)]
        [InlineData(6,3)]
        [InlineData(7,1)]
        public void Should_Get_Log_By_EnvironmentId(int userId, int environmentId)
        {
            var fakeContext = new FakeContext("ControllerGetLogById");
            fakeContext.FillWith<User>();
            fakeContext.FillWith<Log>();
            fakeContext.FillWith<Environment>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var userRepository = new UserRepository(context);
                var validator = new LogValidator();
                var userValidator = new UserValidator(context);
                var service = new LogService(repository, validator, context);
                var userService = new UserService(userRepository, userValidator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new LogController(service, mapper);
                var user = userService.GetById(userId);
                var log = service.GetByEnvironmentId(environmentId);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"log/environment/{environmentId}").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrLog = JsonConvert.DeserializeObject<List<Log>>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<List<Log>>(retrLog);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                foreach (var testLog in retrLog)
                {
                    Assert.IsType<string>(testLog.Name);
                    Assert.IsType<string>(testLog.Description);
                    Assert.IsType<int>(testLog.Id);
                }

            }
        }

        [Fact]
        public void Should_Get_All_Logs()
        {
            var fakeContext = new FakeContext("ControllerGetAllLogs");
            fakeContext.FillWith<User>();
            fakeContext.FillWith<Log>();
            fakeContext.FillWith<Environment>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var userRepository = new UserRepository(context);
                var validator = new LogValidator();
                var userValidator = new UserValidator(context);
                var service = new LogService(repository, validator, context);
                var userService = new UserService(userRepository, userValidator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new LogController(service, mapper);
                var user = userService.GetById(1);
                var log = service.GetAllFull(); 
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"/log").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrLog = JsonConvert.DeserializeObject<List<Log>>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<List<Log>>(retrLog);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                foreach (var testLog in retrLog)
                {
                    Assert.IsType<string>(testLog.Name);
                    Assert.IsType<string>(testLog.Description);
                    Assert.IsType<int>(testLog.Id);
                }

            }
        }
        
        [Theory]
        [InlineData(1,1,1)]
        [InlineData(2,2,2)]
        [InlineData(3,3,3)]
        [InlineData(4,1,1)]
        [InlineData(5,2,2)]
        [InlineData(6,3,3)]
        [InlineData(7,1,1)]
        public void Should_Get_Log_By_Environment_And_Level_Id(int userId, int environmentId, int levelId)
        {
            var fakeContext = new FakeContext("ControllerGetLogByEnvAndLevelId");
            fakeContext.FillWith<User>();
            fakeContext.FillWith<Log>();
            fakeContext.FillWith<Environment>();
            fakeContext.FillWith<Level>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var userRepository = new UserRepository(context);
                var validator = new LogValidator();
                var userValidator = new UserValidator(context);
                var service = new LogService(repository, validator, context);
                var userService = new UserService(userRepository, userValidator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new LogController(service, mapper);
                var user = userService.GetById(userId);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"log/environment/{environmentId}/level/{levelId}").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrLog = JsonConvert.DeserializeObject<List<Log>>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<List<Log>>(retrLog);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                foreach (var testLog in retrLog)
                {
                    Assert.IsType<string>(testLog.Name);
                    Assert.IsType<string>(testLog.Description);
                    Assert.IsType<int>(testLog.Id);
                }

            }
        }
        
        
        [Theory]
        [InlineData(1,1,1)]
        [InlineData(2,2,2)]
        [InlineData(3,3,3)]
        [InlineData(5,2,1)]
        [InlineData(6,3,2)]
        [InlineData(7,1,3)]
        public void Should_Get_Log_By_Environment_And_Layer_Id(int userId, int environmentId, int layerId)
        {
            var fakeContext = new FakeContext("ControllerGetLogByEnvAndLayerId");
            fakeContext.FillWith<User>();
            fakeContext.FillWith<Log>();
            fakeContext.FillWith<Environment>();
            fakeContext.FillWith<Layer>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var userRepository = new UserRepository(context);
                var validator = new LogValidator();
                var userValidator = new UserValidator(context);
                var service = new LogService(repository, validator, context);
                var userService = new UserService(userRepository, userValidator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new LogController(service, mapper);
                var user = userService.GetById(userId);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"log/environment/{environmentId}/layer/{layerId}").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrLog = JsonConvert.DeserializeObject<List<Log>>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<List<Log>>(retrLog);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                foreach (var testLog in retrLog)
                {
                    Assert.IsType<string>(testLog.Name);
                    Assert.IsType<string>(testLog.Description);
                    Assert.IsType<int>(testLog.Id);
                }

            }
        }
        
        [Theory]
        [InlineData(1,1,"tentar")]
        [InlineData(2,2,"ao")]
        [InlineData(3,3,"erro")]
        [InlineData(5,2,"candidato")]
        public void Should_Get_Log_By_EnvironmentId_And_Description(int userId, int environmentId, string description)
        {
            var fakeContext = new FakeContext("ControllerGetLogByEnvIdAndDescription");
            fakeContext.FillWith<User>();
            fakeContext.FillWith<Log>();
            fakeContext.FillWith<Environment>();
            var auth = new UserAuthenticationTest();
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var userRepository = new UserRepository(context);
                var validator = new LogValidator();
                var userValidator = new UserValidator(context);
                var service = new LogService(repository, validator, context);
                var userService = new UserService(userRepository, userValidator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                var controller = new LogController(service, mapper);
                var user = userService.GetById(userId);
                authServer.CreateClient();
                var token = auth.GetToken(user.Email, user.Password);
                
                var client = server.CreateClient();
                client.SetBearerToken(token.access_token);
                var response = client.GetAsync($"/log/environment?environmentId={environmentId}&description={description.ToLower()}").Result;
                var result = response.Content.ReadAsStringAsync();
                var retrLog = JsonConvert.DeserializeObject<List<Log>>(result.Result);

                Assert.IsType<HttpStatusCode>(response.StatusCode);
                Assert.IsType<List<Log>>(retrLog);
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                foreach (var testLog in retrLog)
                {
                    Assert.IsType<string>(testLog.Name);
                    Assert.IsType<string>(testLog.Description);
                    Assert.IsType<int>(testLog.Id);
                }

            }
        }
        
    }
}