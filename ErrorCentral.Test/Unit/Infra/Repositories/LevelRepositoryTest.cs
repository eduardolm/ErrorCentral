using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using Xunit;

namespace ErrorCentral.Tests.Unit.Infra
{
    public class LevelRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Level_In_Db()
        {
            var fakeContext = new FakeContext("GetAllLevel");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var levelCountIndDb = context.Level.Count();
                var repository = new LevelRepository(context);
                
                Assert.Equal(levelCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Level_When_Find_By_Id_In_Db(int id)
        {
            var fakeContext = new FakeContext("LevelById");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Level>().Find(x => x.Id == id);
                var repository = new LevelRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new LevelIdComparer());
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Save_New_Level_To_Db()
        {
            var fakeContext = new FakeContext("AddNewLevel");

            var fakeLevel = new Level();
            fakeLevel.Name = "Desenvolvimento";
            fakeLevel.Id = 4;
           
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                repository.Create(fakeLevel);

                var createdLevel = repository.GetById(4);
                
                Assert.NotEqual(0, fakeLevel.Id);
                Assert.Equal("Desenvolvimento", createdLevel.Name);
                Assert.Equal(4, createdLevel.Id);
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Level_In_Db(int id)
        {
            var fakeContext = new FakeContext("UpdateLevel");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                var currentLevel = repository.GetById(id);

                currentLevel.Name = "123abc";
                repository.Update(currentLevel);
                Assert.Equal("123abc", repository.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Level_In_Db()
        {
            var fakeContext = new FakeContext("DeleteLevel");
            fakeContext.FillWith<Level>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                var currentCount = context.Level.Count();
                var newLevel = new Level();
                newLevel.Name = "Testing";
                context.Level.Add(newLevel);
                context.SaveChanges();
                var idToDelete = (from l in context.Level.ToList()
                    where l.Id == newLevel.Id
                    select l.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, context.Level.Count());
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, context.Level.Count());
                repository.Dispose();
            }
        }
    }
}