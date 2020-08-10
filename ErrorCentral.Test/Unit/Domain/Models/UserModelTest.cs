using System;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using Xunit;

namespace ErrorCentral.Test.Unit.Domain.Models
{
    public class UserModelTest
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
        public void Should_Create_New_User_Properly(int id)
        {
            var fakeContext = new FakeContext("UserModelTest");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var testUser = service.GetById(id);
                
                Assert.NotNull(testUser.FullName);
                Assert.NotNull(testUser.Email);
                Assert.NotNull(testUser.Password);
                Assert.NotEqual(0,testUser.Id);
                Assert.NotNull(testUser.CreatedAt);
                Assert.IsType<DateTime>(testUser.CreatedAt);
                repository.Dispose();
            }
        }
    }
}