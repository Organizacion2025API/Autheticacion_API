using AutoMapper;
using ApexMagnamentAPI.Properties.Models;
using ApexMagnamentAPI.Properties.DTOs;


namespace ApexMagnamentAPI.Properties.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Rol,RolResponse>();
            CreateMap<RolRequest,Rol>();

            CreateMap<Personal, PersonalResponse>();
            CreateMap<PersonalRequest, Personal>();

            CreateMap<Personal, UserResponse>();
            CreateMap<PersonalRequest, Personal>();
        }


    }
}
