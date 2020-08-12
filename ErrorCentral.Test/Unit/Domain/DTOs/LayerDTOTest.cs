using AutoMapper;
using ErrorCentral.Application.Services;
using ErrorCentral.Application.Validators;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;
using ErrorCentral.Infra.Context;
using ErrorCentral.Infra.Repositories;
using ErrorCentral.Test.Unit.Infra.Context;
using ErrorCentral.Web;
using Xunit;

namespace ErrorCentral.Test.Unit.Domain.DTOs
{
    public class LayerDtoTest
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
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testLayer = service.GetById(id);
                var layerDto = mapper.Map<Layer, LayerDto>(testLayer);

                Assert.IsType<LayerDto>(layerDto);
                Assert.Equal(testLayer.Name, layerDto.Name);
            }
        }
    }
}