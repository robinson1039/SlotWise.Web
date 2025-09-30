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
                .ForMember(dest => dest.CreateAt, opt => opt.MapFrom(src => src.Create_at)) // ← ESTA LÍNEA FALTA
                .ReverseMap();
        }
    }
}