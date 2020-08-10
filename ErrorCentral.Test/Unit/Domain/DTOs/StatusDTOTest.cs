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
    public class StatusDtoTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Create_Correct_StatusDTO_Object(int id)
        {
            var fakeContext = new FakeContext("StatusDTOTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new StatusRepository(context);
                var validator = new StatusValidator();
                var service = new StatusService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testStatus = service.GetById(id);
                var statusDto = mapper.Map<Status, StatusDto>(testStatus);

                Assert.IsType<StatusDto>(statusDto);
                Assert.Equal(testStatus.Name, statusDto.Name);
            }
        }
    }
}