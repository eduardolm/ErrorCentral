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
    public class UserDtoTest
    {
        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(5)]
        [InlineData(6)]
        [InlineData(7)]
        public void Should_Create_Correct_UserDTO_Object(int id)
        {
            var fakeContext = new FakeContext("UserDTOTest");
            fakeContext.FillWithAll();

            using (var context = new MainContext(fakeContext.FakeOptions))
            {
                var repository = new UserRepository(context);
                var validator = new UserValidator(context);
                var service = new UserService(repository, validator);
                var mockMapper = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AutoMapperProfile>(); 
                });
                var mapper = mockMapper.CreateMapper();
                
                var testUser = service.GetById(id);
                var userDto = mapper.Map<User, UserDto>(testUser);

                Assert.IsType<UserDto>(userDto);
                Assert.Equal(testUser.FullName, userDto.FullName);
                Assert.Equal(testUser.Email, userDto.Email);
                Assert.Equal(testUser.Id, userDto.Id);
                Assert.Equal(testUser.CreatedAt, userDto.CreatedAt);
            }
        }
    }
}