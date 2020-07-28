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
    public class EnvironmentServiceTest
    {
        [Fact]
        public void Should_Return_All_Environment()
        {
            var fakeContext = new FakeContext("GetAllEnvironment");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var environmentCountIndDb = context.Environment.Count();
                var repository = new EnvironmentRepository(context);
                var validator = new EnvironmentValidator();
                var service = new EnvironmentService(repository, validator);
                
                Assert.Equal(environmentCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Environment_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("EnvironmentById");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Environment>().Find(x => x.Id == id);

                var repository = new EnvironmentRepository(context);
                var validator = new EnvironmentValidator();
                var service = new EnvironmentService(repository, validator);
                var actual = service.GetById(id);

                Assert.Equal(expected, actual, new EnvironmentIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_Environment()
        {
            var fakeContext = new FakeContext("CreateNewEnvironment");

            var fakeEnvironment = new Environment();
            fakeEnvironment.Name = "full name";

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                var mockValidator = new Mock<IValidator<Environment>>(MockBehavior.Strict);
                
                mockValidator
                    .Setup(x => x.Validate(fakeEnvironment))
                    .Returns(new FluentValidation.Results.ValidationResult());

                var service = new EnvironmentService(repository, mockValidator.Object);
                var actual = service.Create(fakeEnvironment);
                var id = actual.Id;

                Assert.NotEqual(0, id);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Existing_Environment(int id)
        {
            var fakeContext = new FakeContext("UpdateEnvironment");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                var validator = new EnvironmentValidator();
                var service = new EnvironmentService(repository, validator);
                var curretEnvironment = service.GetById(id);

                curretEnvironment.Name = "Testing";
                service.Update(curretEnvironment);
                Assert.Equal("Testing", service.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Environment()
        {
            var fakeContext = new FakeContext("DeleteEnvironment");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                var validator = new EnvironmentValidator();
                var service = new EnvironmentService(repository, validator);
                var currentCount = context.Environment.Count();
                
                Assert.NotEqual(0, currentCount);
                service.Delete(1);
                Assert.NotEqual(currentCount,context.Environment.Count());
                repository.Dispose();
            }
        }
    }
}