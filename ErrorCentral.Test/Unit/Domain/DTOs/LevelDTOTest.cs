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
    public class LevelDTOTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Create_Correct_LevelDTO_Object(int id)
        {
            var fakeContext = new FakeContext("LevelDTOTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new LevelRepository(context);
                var validator = new LevelValidator();
                var service = new LevelService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testLevel = service.GetById(id);
                var levelDTO = mapper.Map<Level, LevelDTO>(testLevel);

                Assert.IsType<LevelDTO>(levelDTO);
                Assert.Equal(testLevel.Name, levelDTO.Name);
            }
        }
    }
}