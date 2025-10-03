using AutoMapper;
using SlotWise.Web.Data.Entities;
using SlotWise.Web.DTOs;

namespace SlotWise.Web.Core
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Specialist, SpecialistDTO>()
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.Create_at))
                .ReverseMap();
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.CreateAt))
                .ReverseMap();
            CreateMap<Service, ServiceDTO>()
               .ForMember(dest => dest.SpecialistName, opt => opt.MapFrom(src => src.Specialist != null ? src.Specialist.FirstName: string.Empty))
               .ReverseMap()
               .ForMember(dest => dest.Specialist, opt => opt.Ignore()); // Ignorar la navegación al mapear de vuelta


        }
    }
}