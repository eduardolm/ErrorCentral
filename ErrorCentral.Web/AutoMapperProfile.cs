using AutoMapper;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Log, LogDto>().ReverseMap();
            CreateMap<Layer, LayerDto>().ReverseMap();
            CreateMap<Level, LevelDto>().ReverseMap();
            CreateMap<Status, StatusDto>().ReverseMap();
            CreateMap<Environment, EnvironmentDto>().ReverseMap();
        }
    }
}