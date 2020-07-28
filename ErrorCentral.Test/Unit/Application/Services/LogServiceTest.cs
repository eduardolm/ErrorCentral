using System.Collections.Generic;
using System.Linq;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using ErrorCentral.Tests.Unit.Infra;
using FluentAssertions;
using FluentValidation;
using Moq;
using Xunit;

namespace ErrorCentral.Test.Unit.Application.Services
{
    public class LogServiceTest
    {
        [Fact]
        public void Should_Return_All_Log()
        {
            var fakeContext = new FakeContext("GetAllLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var logCountIndDb = context.Log.Count();
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                
                Assert.Equal(logCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
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
        [InlineData(11)]
        [InlineData(12)]
        public void Should_Return_Right_Log_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("LogById");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Log>().Find(x => x.Id == id);

                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var actual = service.GetById(id);
                var tryZeroId = service.GetById(0);

                Assert.Null(tryZeroId);
                Assert.Equal(expected, actual, new LogIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_Log()
        {
            var fakeContext = new FakeContext("CreateNewLog");

            var fakeLog = new Log();
            fakeLog.Name = "full name";

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var mockValidator = new Mock<IValidator<Log>>(MockBehavior.Strict);
                
                mockValidator
                    .Setup(x => x.Validate(fakeLog))
                    .Returns(new FluentValidation.Results.ValidationResult());

                var service = new LogService(repository, mockValidator.Object, context);
                var actual = service.Create(fakeLog);
                var id = actual.Id;

                Assert.NotEqual(0, id);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Existing_Log(int id)
        {
            var fakeContext = new FakeContext("UpdateLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var curretLog = service.GetById(id);

                curretLog.Name = "Testing";
                service.Update(curretLog);
                Assert.Equal("Testing", service.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Log()
        {
            var fakeContext = new FakeContext("DeleteLogLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var currentCount = context.Log.Count();
                
                Assert.NotEqual(0, currentCount);
                service.Delete(1);
                Assert.NotEqual(currentCount,context.Log.Count());
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Get_By_EnvironmentId(int id)
        {
            var fakeContext = new FakeContext("DeleteLogLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var logByEnvId = service.GetByEnvironmentId(id);

                foreach (var log in logByEnvId)
                {
                    Assert.Equal(id, log.EnvironmentId);
                }
            }
        }

        [Fact]
        public void Should_GetFullLog()
        {
            var fakeContext = new FakeContext("DeleteLogLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var logs = service.GetAllFull();

                foreach (var log in logs)
                {
                    Assert.NotEqual(0, log.Id);
                }
                Assert.NotEmpty(logs);
            }
        }
        
        [Theory]
        [InlineData(1,1)]
        [InlineData(2,2)]
        [InlineData(3,3)]
        [InlineData(1,2)]
        [InlineData(1,3)]
        [InlineData(2,1)]
        [InlineData(2,3)]
        [InlineData(3,1)]
        [InlineData(3,2)]
        public void Should_Get_By_Environment_And_Level_Id(int environmentId, int levelId)
        {
            var fakeContext = new FakeContext("DeleteLogLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var logByEnvIdAndLevelId = service.GetByEnvironmentIdAndLevel(environmentId, levelId);

                foreach (var log in logByEnvIdAndLevelId)
                {
                    Assert.NotEqual(0, log.Id);
                    Assert.IsType<Log>(log);
                }
            }
        }

        [Theory]
        [InlineData(1, 1)]
        [InlineData(1, 2)]
        [InlineData(1, 3)]
        [InlineData(1, 4)]
        [InlineData(2, 1)]
        [InlineData(2, 2)]
        [InlineData(2, 3)]
        [InlineData(2, 4)]
        [InlineData(3, 1)]
        [InlineData(3, 2)]
        [InlineData(3, 3)]
        [InlineData(3, 4)]
        [InlineData(4, 1)]
        [InlineData(4, 2)]
        [InlineData(4, 3)]
        [InlineData(4, 4)]
        public void Should_Get_By_Environment_and_Layer_Id(int environmentId, int layerId)
        {
            var fakeContext = new FakeContext("DeleteLogLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var logByEnvAndLayerId = service.GetByEnvironmentAndLayer(environmentId, layerId);

                foreach (var log in logByEnvAndLayerId)
                {
                    Assert.NotEqual(0, log.Id);
                    Assert.IsType<Log>(log);
                }
            }
        }

        [Theory]
        [InlineData(1, "tentar")]
        [InlineData(2, "tentar")]
        [InlineData(3, "tentar")]
        public void Should_Get_By_Environment_And_Description(int environmentId, string description)
        {
            var fakeContext = new FakeContext("DeleteLogLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var validator = new LogValidator();
                var service = new LogService(repository, validator, context);
                var logByEnvAndDescriptionId = service
                    .GetByEnvironmentAndDescription(environmentId, description);

                foreach (var log in logByEnvAndDescriptionId)
                {
                    Assert.Contains("tentar", log.Description);
                }
                Assert.NotEmpty(logByEnvAndDescriptionId);
                Assert.NotNull(logByEnvAndDescriptionId);
            }
        }
    }
}