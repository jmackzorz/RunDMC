using AutoMapper;
using RunDMC.DTOs;
using RunDMC.Models;

namespace RunDMC.Mappings;

public class FitnessProfile : Profile
{
    public FitnessProfile()
    {
        CreateMap<Workout, WorkoutDto>()
            .ForMember(dest => dest.ActivityTypeName,
                       opt => opt.MapFrom(src => src.ActivityType.Name));

        CreateMap<CreateWorkoutDto, Workout>();
    }
}
