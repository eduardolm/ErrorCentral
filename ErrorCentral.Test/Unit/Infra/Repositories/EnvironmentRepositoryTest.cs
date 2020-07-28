using System.Linq;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using Xunit;
using Environment = ErrorCentral.Domain.Models.Environment;

namespace ErrorCentral.Tests.Unit.Infra
{
    public class EnvironmentRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Environment_In_Db()
        {
            var fakeContext = new FakeContext("GetAllEnvs");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var envCountIndDb = context.Environment.Count();
                var repository = new EnvironmentRepository(context);
                
                Assert.Equal(envCountIndDb, repository.GetAll().Count());
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Environment_When_Find_By_Id_In_Db(int id)
        {
            var fakeContext = new FakeContext("EnvironmentById");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Environment>().Find(x => x.Id == id);
                var repository = new EnvironmentRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new EnvironmentIdComparer());
            }
        }
        
        [Fact]
        public void Should_Save_New_Environemnt_To_Db()
        {
            var fakeContext = new FakeContext("AddNewEnvironemnt");

            var fakeEnv = new Environment();
            fakeEnv.Name = "Desenvolvimento";
            fakeEnv.Id = 4;
           
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                repository.Create(fakeEnv);

                var createdEnv = repository.GetById(4);
                
                Assert.NotEqual(0, fakeEnv.Id);
                Assert.Equal("Desenvolvimento", createdEnv.Name);
                Assert.Equal(4, createdEnv.Id);
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Environment_In_Db(int id)
        {
            var fakeContext = new FakeContext("UpdateEnv");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                var currentEnv = repository.GetById(id);

                currentEnv.Name = "123abc";
                repository.Update(currentEnv);
                Assert.Equal("123abc", repository.GetById(id).Name);
            }
        }

        [Fact]
        public void Should_Delete_Environment_In_Db()
        {
            var fakeContext = new FakeContext("DeleteEnv");
            fakeContext.FillWith<Environment>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                var currentCount = context.Environment.Count();

                Assert.Equal(3, currentCount);
                repository.Delete(1);
                Assert.NotEqual(3, context.Environment.Count());
            }
        }
    }
}