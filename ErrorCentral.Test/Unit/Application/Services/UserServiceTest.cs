using System;
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
    public class UserServiceTest
    {
        [Fact]
        public void Should_Return_All_Users()
        {
            var fakeContext = new FakeContext("GetAllUsers");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.User.Count();
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                
                Assert.Equal(userCountIndDb, service.GetAll().Count());
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
        public void Should_Return_Right_User_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("UserById");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<User>().Find(x => x.Id == id);

                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var actual = service.GetById(id);
                var tryZeroIdUser = service.GetById(0);

                Assert.Null(tryZeroIdUser);
                Assert.Equal(expected, actual, new UserIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_User()
        {
            var fakeContext = new FakeContext("CreateNewUser");

            var fakeUser = new User();
            fakeUser.FullName = "full name";
            fakeUser.Email = "email";
            fakeUser.Password = "pass";
            fakeUser.CreatedAt = new DateTime(2019, 03, 14, 15, 34, 02);

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var mockValidator = new Mock<IValidator<User>>(MockBehavior.Strict);
                
                mockValidator
                    .Setup(x => x.Validate(fakeUser))
                    .Returns(new FluentValidation.Results.ValidationResult());

                var service = new UserService(repository, mockValidator.Object);
                var actual = service.Create(fakeUser);
                var id = actual.Id;

                Assert.NotEqual(0, id);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Existing_User(int id)
        {
            var fakeContext = new FakeContext("UpdateUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var currentUser = service.GetById(id);

                currentUser.Email = "email@test.com";
                service.Update(currentUser);
                Assert.Equal("email@test.com", service.GetById(id).Email);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_User()
        {
            var fakeContext = new FakeContext("DeleteUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var currentCount = context.User.Count();
                var newUser = new User();
                newUser.FullName = "New user";
                newUser.Email = "user@mail.com";
                newUser.Password = "strongPass";
                service.Create(newUser);
                var createdUser = (from u in service.GetAll()
                    where u.Id != 0
                    select u).FirstOrDefault();

                Assert.Throws<ArgumentNullException>(() => service.Delete(0));
                Assert.NotEqual(0, currentCount);
                service.Delete(createdUser.Id);
                Assert.Equal(currentCount,context.User.Count());
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
        public void Should_Not_Create_Repeated_User(int id)
        {
            var fakeContext = new FakeContext("DeleteUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);

                var newUser = new User();
                newUser.FullName = "Test Name";
                newUser.Email = "test@mail.com";
                newUser.Password = "testPass";
                var result = service.Create(newUser);
                var existingUser = (from u in service.GetAll()
                    where u.Id == id
                    select u).FirstOrDefault();

                var anotherUser = new User();
                anotherUser.FullName = "Test Name";
                anotherUser.Email = "test@mail.com";
                anotherUser.Password = "testPass";
                var anotherResult = service.Create(anotherUser);
                
                Assert.Null(anotherResult);
                Assert.NotEqual(newUser.Id, existingUser.Id);
                Assert.NotEqual(newUser.FullName, existingUser.FullName);
                repository.Dispose();
            }
        }
    }
}