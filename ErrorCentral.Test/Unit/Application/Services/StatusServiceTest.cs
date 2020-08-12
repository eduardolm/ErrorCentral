using System.Linq;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Domain;
using ErrorCentral.Test.Unit.Infra.Context;
using FluentValidation;
using Moq;
using Xunit;

namespace ErrorCentral.Test.Unit.Application.Services
{
    public class StatusServiceTest
    {
        [Fact]
        public void Should_Return_All_Status()
        {
            var fakeContext = new FakeContext("GetAllStatus");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var statusCountIndDb = context.Status.Count();
                var repository = new StatusRepository(context);
                var validator = new StatusValidator();
                var service = new StatusService(repository, validator);
                
                Assert.Equal(statusCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Status_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("StatusById");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Status>().Find(x => x.Id == id);

                var repository = new StatusRepository(context);
                var validator = new StatusValidator();
                var service = new StatusService(repository, validator);
                var actual = service.GetById(id);

                Assert.Equal(expected, actual, new StatusIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_Status()
        {
            var fakeContext = new FakeContext("CreateNewStatus");

            var fakeStatus = new Status();
            fakeStatus.Name = "full name";

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                var mockValidator = new Mock<IValidator<Status>>(MockBehavior.Strict);
                
                mockValidator
                    .Setup(x => x.Validate(fakeStatus))
                    .Returns(new FluentValidation.Results.ValidationResult());

                var service = new StatusService(repository, mockValidator.Object);
                var actual = service.Create(fakeStatus);
                var id = actual.Id;

                Assert.NotEqual(0, id);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Existing_Status(int id)
        {
            var fakeContext = new FakeContext("UpdateStatus");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                var validator = new StatusValidator();
                var service = new StatusService(repository, validator);
                var curretStatus = service.GetById(id);

                curretStatus.Name = "Testing";
                service.Update(curretStatus);
                Assert.Equal("Testing", service.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Status()
        {
            var fakeContext = new FakeContext("DeleteStatus");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                var validator = new StatusValidator();
                var service = new StatusService(repository, validator);
                var currentCount = context.Status.Count();
                var newStatus = new Status();
                newStatus.Name = "Testing";
                service.Create(newStatus);
                var createdStatus = (from u in service.GetAll()
                    where u.Id != 0
                    select u).FirstOrDefault();

                Assert.NotEqual(0, currentCount);
                service.Delete(createdStatus.Id);
                Assert.Equal(currentCount,context.Status.Count());
                repository.Dispose();
            }
        }
    }
}