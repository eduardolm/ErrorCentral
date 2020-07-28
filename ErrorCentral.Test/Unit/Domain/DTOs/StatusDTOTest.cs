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
    public class StatusDTOTest
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
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testStatus = service.GetById(id);
                var statusDTO = mapper.Map<Status, StatusDTO>(testStatus);

                Assert.IsType<StatusDTO>(statusDTO);
                Assert.Equal(testStatus.Name, statusDTO.Name);
            }
        }
    }
}