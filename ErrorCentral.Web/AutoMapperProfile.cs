using AutoMapper;
using ErrorCentral.Domain.DTOs;
using ErrorCentral.Domain.Models;

namespace ErrorCentral.Web
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Status, StatusDTO>().ReverseMap();
            CreateMap<Log, LogDTO>().ReverseMap();
            CreateMap<Layer, LayerDTO>().ReverseMap();
            CreateMap<Level, LevelDTO>().ReverseMap();
            CreateMap<Status, StatusDTO>().ReverseMap();
            CreateMap<Environment, EnvironmentDTO>().ReverseMap();
        }
    }
}