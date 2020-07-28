using AutoMapper;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Infra;
using ErrorCentral.Web;
using Xunit;

namespace ErrorCentral.Tests.Unit.Domain.DTOs
{
    public class LogDTOTest
    {
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
        [InlineData(11)]
        [InlineData(12)]
        public void Should_Create_Correct_LogDTO_Object(int id)
        {
            var fakeContext = new FakeContext("LogDTOTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testLog = service.GetFullLog(id);
                var logDTO = mapper.Map<Log, LogDTO>(testLog);

                Assert.IsType<LogDTO>(logDTO);
                Assert.Equal(testLog.Id, logDTO.Id);
                Assert.Equal(testLog.Name, logDTO.Name);
                Assert.Equal(testLog.Description, logDTO.Description);
                Assert.Equal(testLog.Layer.Name, logDTO.Layer.Name);
                Assert.Equal(testLog.Level.Name, logDTO.Level.Name);
                Assert.Equal(testLog.Status.Name, logDTO.Status.Name);
                Assert.Equal(testLog.Environment.Name, logDTO.Environment.Name);
                Assert.Equal(testLog.CreatedAt, logDTO.CreatedAt);
                Assert.Equal(testLog.User.FullName, logDTO.User.FullName);
            }
        }
    }
}