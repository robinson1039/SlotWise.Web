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
            CreateMap<Reservation, ReservationDTO>()
               .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User != null ? src.User.FirstName : string.Empty))
               .ForMember(dest => dest.SpecialistName, opt => opt.MapFrom(src => src.Specialist != null ? src.Specialist.FirstName : string.Empty))
               .ForMember(dest => dest.ServiceName, opt => opt.MapFrom(src => src.Service != null ? src.Service.NameService : string.Empty))
               .ReverseMap()
               .ForMember(dest => dest.User, opt => opt.Ignore()) // Ignorar la navegación al mapear de vuelta
               .ForMember(dest => dest.Specialist, opt => opt.Ignore()) // Ignorar la navegación al mapear de vuelta
               .ForMember(dest => dest.Service, opt => opt.Ignore()); // Ignorar la navegación al mapear de vuelta

        }
    }
}