using AutoMapper;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Tests.Unit.Infra;
using ErrorCentral.Web;
using Xunit;

namespace ErrorCentral.Tests.Unit.Domain.DTOs
{
    public class LayerDTOTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        public void Should_Create_Correct_LayerDTO_Object(int id)
        {
            var fakeContext = new FakeContext("LayerDTOTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LayerRepository(context);
                var validator = new LayerValidator();
                var service = new LayerService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testLayer = service.GetById(id);
                var layerDTO = mapper.Map<Layer, LayerDTO>(testLayer);

                Assert.IsType<LayerDTO>(layerDTO);
                Assert.Equal(testLayer.Name, layerDTO.Name);
            }
        }
    }
}