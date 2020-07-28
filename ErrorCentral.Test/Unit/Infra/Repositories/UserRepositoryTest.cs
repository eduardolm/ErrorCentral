using System;
using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using Xunit;

namespace ErrorCentral.Tests.Unit.Infra
{
    public class UserRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Users_In_Db()
        {
            var fakeContext = new FakeContext("GetAllUsers");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var userCountIndDb = context.User.Count();
                var repository = new UserRepository(context);
                
                Assert.Equal(userCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_User_When_Find_By_Id_In_Db(int id)
        {
            var fakeContext = new FakeContext("UserById");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<User>().Find(x => x.Id == id);
                var repository = new UserRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new UserIdComparer());
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Save_New_User_To_Db()
        {
            var fakeContext = new FakeContext("AddNewUser");

            var fakeUser = new User();
            fakeUser.FullName = "full name";
            fakeUser.Email = "email";
            fakeUser.Password = "pass";
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                repository.Create(fakeUser);

                var createdUser = repository.GetById(1);
                
                Assert.NotEqual(0, fakeUser.Id);
                Assert.Equal("full name", createdUser.FullName);
                Assert.Equal("email", createdUser.Email);
                Assert.Equal("pass", createdUser.Password);
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_User_In_Db(int id)
        {
            var fakeContext = new FakeContext("UpdateUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var currentUser = repository.GetById(id);

                currentUser.Password = "123abc";
                repository.Update(currentUser);
                Assert.Equal("123abc", repository.GetById(id).Password);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_User_In_Db()
        {
            var fakeContext = new FakeContext("DeleteUser");
            fakeContext.FillWith<User>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var currentCount = context.User.Count();
                var newUser = new User();
                newUser.FullName = "Test User";
                newUser.Email = "test@mail.com";
                newUser.Password = "pass123";
                repository.Create(newUser);
                var idToDelete = (from u in repository.GetAll()
                    where u.Id == newUser.Id
                    select u.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, repository.GetAll().ToList().Count);
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, repository.GetAll().ToList().Count());
                repository.Dispose();
            }
        }
    }
        
}
