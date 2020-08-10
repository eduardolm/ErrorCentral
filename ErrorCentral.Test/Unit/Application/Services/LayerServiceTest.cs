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
    public class LayerServiceTest
    {
        [Fact]
        public void Should_Return_All_Layers()
        {
            var fakeContext = new FakeContext("GetAllLayers");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var layerCountIndDb = context.Layer.Count();
                var repository = new LayerRepository(context);
                var validator = new LayerValidator();
                var service = new LayerService(repository, validator);
                
                Assert.Equal(layerCountIndDb, service.GetAll().Count());
                repository.Dispose();
            }
        }
        
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Should_Return_Right_Layer_When_Find_By_Id(int id)
        {
            var fakeContext = new FakeContext("LayerById");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var expected = fakeContext.GetFakeData<Layer>().Find(x => x.Id == id);

                var repository = new LayerRepository(context);
                var validator = new LayerValidator();
                var service = new LayerService(repository, validator);
                var actual = service.GetById(id);

                Assert.Equal(expected, actual, new LayerIdComparer());
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Create_New_Layer()
        {
            var fakeContext = new FakeContext("CreateNewLayer");

            var fakeLayer = new Layer();
            fakeLayer.Name = "full name";

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                var mockValidator = new Mock<IValidator<Layer>>(MockBehavior.Strict);
                
                mockValidator
                    .Setup(x => x.Validate(fakeLayer))
                    .Returns(new FluentValidation.Results.ValidationResult());

                var service = new LayerService(repository, mockValidator.Object);
                var actual = service.Create(fakeLayer);
                var id = actual.Id;

                Assert.NotEqual(0, id);
                repository.Dispose();
            }
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Update_Existing_Layer(int id)
        {
            var fakeContext = new FakeContext("UpdateLayer");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                var validator = new LayerValidator();
                var service = new LayerService(repository, validator);
                var curretLayer = service.GetById(id);

                curretLayer.Name = "Testing";
                service.Update(curretLayer);
                Assert.Equal("Testing", service.GetById(id).Name);
                repository.Dispose();
            }
        }

        [Fact]
        public void Should_Delete_Layer()
        {
            var fakeContext = new FakeContext("DeleteLayer");
            fakeContext.FillWith<Layer>();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                var validator = new LayerValidator();
                var service = new LayerService(repository, validator);
                var currentCount = context.Layer.Count();
                
                Assert.NotEqual(0, currentCount);
                service.Delete(1);
                Assert.NotEqual(currentCount,context.Layer.Count());
                repository.Dispose();
            }
        }
    }
}