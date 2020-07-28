using System;
using System.Linq;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Infra;
using Xunit;

namespace ErrorCentral.Tests.Unit.Domain.Models
{
    public class LogModelTest
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
        public void Should_Get_Log_Info_Right(int id)
        {
            var fakeContext = new FakeContext("LogInfo");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var logbuilder = new LogBuilder(context);
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var logTest = service.GetById(id);
                
                Assert.NotEqual(0, logTest.UserId);
                Assert.NotNull(logbuilder.GetPayload(logTest));
                Assert.NotEqual(0, logTest.EnvironmentId);
                Assert.NotEqual(0, logTest.LayerId);
                Assert.NotEqual(0,logTest.LevelId);
                Assert.NotEqual(0,logTest.StatusId);
                context.Dispose();
            }
        }

        [Fact]
        public void Should_Get_Log_Properties_After_Creation()
        {
            var fakeContext = new FakeContext("LogGetter");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var logbuilder = new LogBuilder(context);
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                
                var logTest = new Log();
                logTest.Name = "Error testing";
                logTest.Description = "Testing log getter and setters";
                logTest.UserId = 1;
                logTest.LevelId = 2;
                logTest.LayerId = 2;
                logTest.EnvironmentId = 2;
                logTest.StatusId = 1;
                logTest.CreatedAt = new DateTime(2020, 05, 21, 14, 35, 01);
                service.Create(logTest);

                var createdLog = (from l in service.GetAllFull()
                    where l.Name == "Error testing"
                    select l).FirstOrDefault();
                
                Assert.Equal("Error testing", createdLog.Name);
                Assert.Equal("Testing log getter and setters", createdLog.Description);
                Assert.Equal(1, createdLog.UserId);
                Assert.Equal(2, createdLog.LevelId);
                Assert.Equal(2, createdLog.LayerId);
                Assert.Equal(2, createdLog.EnvironmentId);
                Assert.Equal(1, createdLog.StatusId);
                Assert.Equal(new DateTime(2020, 05, 21, 14, 35, 01), createdLog.CreatedAt );
                Assert.Equal("José da Silva", createdLog.User.FullName);
                Assert.Equal("Warning", createdLog.Level.Name);
                Assert.Equal("Frontend", createdLog.Layer.Name);
                Assert.Equal("Homologação", createdLog.Environment.Name);
                Assert.Equal("Arquivado", createdLog.Status.Name);
                context.Dispose();
            }
        }
    }
}