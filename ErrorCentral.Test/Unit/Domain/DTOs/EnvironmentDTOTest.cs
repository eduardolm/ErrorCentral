﻿using AutoMapper;
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
    public class EnvironmentDTOTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void Should_Create_Correct_EnvironmentDTO_Object(int id)
        {
            var fakeContext = new FakeContext("EnvironmentDTOTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new EnvironmentRepository(context);
                var validator = new EnvironmentValidator();
                var service = new EnvironmentService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>();; 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testEnv = service.GetById(id);
                var envDTO = mapper.Map<Environment, EnvironmentDTO>(testEnv);

                Assert.IsType<EnvironmentDTO>(envDTO);
                Assert.Equal(testEnv.Name, envDTO.Name);
            }
        }
    }
}
