using System.Linq;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using ErrorCentral.Tests.Unit.Infra;
using FluentValidation;
using Moq;
using Xunit;

namespace ErrorCentral.Test.Unit.Application.Services
{
    public class LevelServiceTest
    {
        [Fact]
        public void Should_Return_All_Levels()
        {
            var fakeContext = new FakeContext("GetAllLevels");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var levelCountIndDb = context.Level.Count();
                var repository = new LevelRepository(context);
                var validator = new LevelValidator();
                var service = new LevelService(repository, validator);
                
                Assert.Equal(levelCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Level_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("LevelById");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Level>().Find(x => x.Id == id);

                var repository = new LevelRepository(context);
                var validator = new LevelValidator();
                var service = new LevelService(repository, validator);
                var actual = service.GetById(id);

                Assert.Equal(expected, actual, new LevelIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_Level()
        {
            var fakeContext = new FakeContext("CreateNewLevel");

            var fakeLevel = new Level();
            fakeLevel.Name = "full name";

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                var mockValidator = new Mock<IValidator<Level>>(MockBehavior.Strict);
                
                mockValidator
                    .Setup(x => x.Validate(fakeLevel))
                    .Returns(new FluentValidation.Results.ValidationResult());

                var service = new LevelService(repository, mockValidator.Object);
                var actual = service.Create(fakeLevel);
                var id = actual.Id;

                Assert.NotEqual(0, id);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Existing_Level(int id)
        {
            var fakeContext = new FakeContext("UpdateLevel");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                var validator = new LevelValidator();
                var service = new LevelService(repository, validator);
                var curretLevel = service.GetById(id);

                curretLevel.Name = "Testing";
                service.Update(curretLevel);
                Assert.Equal("Testing", service.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Level()
        {
            var fakeContext = new FakeContext("DeleteLevel");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                var validator = new LevelValidator();
                var service = new LevelService(repository, validator);
                var currentCount = context.Level.Count();
                
                Assert.NotEqual(0, currentCount);
                service.Delete(1);
                Assert.NotEqual(currentCount,context.Level.Count());
                repository.Dispose();
            }
        }
    }
}