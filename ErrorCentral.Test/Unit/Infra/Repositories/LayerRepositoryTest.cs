using System.Linq;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Domain;
using Xunit;

namespace ErrorCentral.Tests.Unit.Infra
{
    public class LayerRepositoryTest
    {
        [Fact]
        public void Should_Return_All_Layer_In_Db()
        {
            var fakeContext = new FakeContext("GetAllLayer");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var layerCountIndDb = context.Layer.Count();
                var repository = new LayerRepository(context);
                
                Assert.Equal(layerCountIndDb, repository.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Return_Right_Layer_When_Find_By_Id_In_Db(int id)
        {
            var fakeContext = new FakeContext("LayerById");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Layer>().Find(x => x.Id == id);
                var repository = new LayerRepository(context);
                var actual = repository.GetById(id);
                
                Assert.Equal(expected, actual, new LayerIdComparer());
                repository.Dispose();
            }
        }
        
        [Fact]
        public void Should_Save_New_Layer_To_Db()
        {
            var fakeContext = new FakeContext("AddNewLayer");

            var fakeLayer = new Layer();
            fakeLayer.Name = "Desenvolvimento";
            fakeLayer.Id = 4;
           
            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                repository.Create(fakeLayer);

                var createdLayer = repository.GetById(4);
                
                Assert.NotEqual(0, fakeLayer.Id);
                Assert.Equal("Desenvolvimento", createdLayer.Name);
                Assert.Equal(4, createdLayer.Id);
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Layer_In_Db(int id)
        {
            var fakeContext = new FakeContext("UpdateLayer");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                var currentLayer = repository.GetById(id);

                currentLayer.Name = "123abc";
                repository.Update(currentLayer);
                Assert.Equal("123abc", repository.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Layer_In_Db()
        {
            var fakeContext = new FakeContext("DeleteLayer");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                var currentCount = context.Layer.Count();
                var newLayer = new Layer();
                newLayer.Name = "Layer";
                repository.Create(newLayer);
                var idToDelete = (from l in repository.GetAll()
                    where l.Id == newLayer.Id
                    select l.Id).FirstOrDefault();

                Assert.Equal(currentCount + 1, repository.GetAll().Count());
                repository.Delete(idToDelete);
                Assert.Equal(currentCount, context.Layer.Count());
                repository.Dispose();
            }
        }
    }
}