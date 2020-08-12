using AutoMapper;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Web;
using Xunit;

namespace ErrorCentral.Test.Unit.Domain.DTOs
{
    public class LogDtoTest
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
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testLog = service.GetFullLog(id);
                var logDto = mapper.Map<Log, LogDto>(testLog);

                Assert.IsType<LogDto>(logDto);
                Assert.Equal(testLog.Id, logDto.Id);
                Assert.Equal(testLog.Name, logDto.Name);
                Assert.Equal(testLog.Description, logDto.Description);
                Assert.Equal(testLog.Layer.Name, logDto.Layer.Name);
                Assert.Equal(testLog.Level.Name, logDto.Level.Name);
                Assert.Equal(testLog.Status.Name, logDto.Status.Name);
                Assert.Equal(testLog.Environment.Name, logDto.Environment.Name);
                Assert.Equal(testLog.CreatedAt, logDto.CreatedAt);
                Assert.Equal(testLog.User.FullName, logDto.User.FullName);
            }
        }
    }
}