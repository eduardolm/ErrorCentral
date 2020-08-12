using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Domain;
using ErrorCentral.Test.Unit.Infra.Context;
using Xunit;

namespace ErrorCentral.Test.Unit.Infra.Repositories
{
    public class LogRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Logs_In_Db()
        {
            var fakeContext = new FakeContext("GetAllLogs");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var logCountIndDb = context.Log.Count();
                var repository = new LogRepository(context);
                
                Assert.Equal(logCountIndDb, repository.GetAll().Count());
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Log_When_Find_By_Id_In_Db(int id)
        {
            var fakeContext = new FakeContext("LogById");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Log>().Find(x => x.Id == id);
                var repository = new LogRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new LogIdComparer());
            }
        }
        
        [Fact]
        public void Should_Save_New_Log_To_Db()
        {
            var fakeContext = new FakeContext("AddNewLog");

            var fakeLog = new Log();
            fakeLog.Name = "Error. File not able to update.";
            fakeLog.Description =
                "File failed to update. There was an error while updating the file. User should contact system administrator.";
            fakeLog.LayerId = 1;
            fakeLog.LevelId = 2;
            fakeLog.StatusId = 3;
            fakeLog.EnvironmentId = 1;
            
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                repository.Create(fakeLog);

                var createdLog = repository.GetById(1);
                
                Assert.NotEqual(0, fakeLog.Id);
                Assert.Equal("Error. File not able to update.", createdLog.Name);
                Assert.Equal("File failed to update. There was an error while updating the file. User should contact system administrator.", createdLog.Description);
                Assert.Equal(2, createdLog.LevelId);
                Assert.Equal(1, createdLog.LayerId);
                Assert.Equal(3, createdLog.StatusId);
                Assert.Equal(1, createdLog.EnvironmentId);
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Log_In_Db(int id)
        {
            var fakeContext = new FakeContext("UpdateLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var currentLog = repository.GetById(id);

                currentLog.Name = "123abc";
                repository.Update(currentLog);
                Assert.Equal("123abc", repository.GetById(id).Name);
            }
        }

        [Fact]
        public void Should_Delete_Log_In_Db()
        {
            var fakeContext = new FakeContext("DeleteLog");
            fakeContext.FillWith<Log>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LogRepository(context);
                var currentCount = context.Log.Count();

                Assert.Equal(40, currentCount);
                repository.Delete(1);
                Assert.NotEqual(40, context.Log.Count());
            }
        }
    }
}