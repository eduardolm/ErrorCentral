using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using Xunit;

namespace ErrorCentral.Tests.Unit.Infra
{
    public class StatusRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Status_In_Db()
        {
            var fakeContext = new FakeContext("GetAllStatus");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var statusCountIndDb = context.Status.Count();
                var repository = new StatusRepository(context);
                
                Assert.Equal(statusCountIndDb, repository.GetAll().Count());
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Status_When_Find_By_Id_In_Db(int id)
        {
            var fakeContext = new FakeContext("StatusById");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Status>().Find(x => x.Id == id);
                var repository = new StatusRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new StatusIdComparer());
            }
        }
        
        [Fact]
        public void Should_Save_New_Status_To_Db()
        {
            var fakeContext = new FakeContext("AddNewStatus");

            var fakeStatus = new Status();
            fakeStatus.Name = "Desenvolvimento";
            fakeStatus.Id = 4;
           
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                repository.Create(fakeStatus);

                var createdStatus = repository.GetById(4);
                
                Assert.NotEqual(0, fakeStatus.Id);
                Assert.Equal("Desenvolvimento", createdStatus.Name);
                Assert.Equal(4, createdStatus.Id);
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Status_In_Db(int id)
        {
            var fakeContext = new FakeContext("UpdateStatus");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                var currentStatus = repository.GetById(id);

                currentStatus.Name = "123abc";
                repository.Update(currentStatus);
                Assert.Equal("123abc", repository.GetById(id).Name);
            }
        }

        [Fact]
        public void Should_Delete_Status_In_Db()
        {
            var fakeContext = new FakeContext("DeleteStatus");
            fakeContext.FillWith<Status>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                var currentCount = context.Status.Count();
                var newStatus = new Status();
                newStatus.Name = "Testing";
                context.Status.Add(newStatus);
                context.SaveChanges();
                var idToDelete = (from s in context.Status.ToList()
                    where s.Id == newStatus.Id
                    select s.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, context.Status.Count());
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, context.Status.Count());
            }
        }
    }
}