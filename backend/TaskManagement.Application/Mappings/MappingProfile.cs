using AutoMapper;
using TaskManagement.Application.DTOs;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Domain to DTO
        CreateMap<TaskItem, TaskResponse>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
    }
}