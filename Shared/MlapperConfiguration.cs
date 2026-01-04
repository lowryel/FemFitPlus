using System;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;

// using FemFitPlus.DTOs;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Shared;

public class MlapperConfiguration
{
    public static class MapperConfig
    {
        public static IMapper InitializeAutoMapper()
        {
            var config = new MapperConfiguration();
                // Add your mappings here
                config.CreateMap<ConsultationDto, Consultation>();
                config.CreateMap<FemFitUserDto, FemFitUser>();
                config.CreateMap<CycleDto, Cycle>().ReverseMap();
                // config.CreateMap<Cycle, CycleDto>();
                config.CreateMap<CycleCreateDto, Cycle>().ReverseMap();
                // config.CreateMap<Cycle, CycleCreateDto>();
                config.CreateMap<CycleUpdateDto, Cycle>().ReverseMap();
                // config.ReverseMap<CycleUpdateDto, Cycle>();
                // config.CreateMap<Cycle, CycleUpdateDto>();
                config.CreateMap<ProfileCreateDto, Profile>().ReverseMap();
                config.CreateMap<ProfileDto, Profile>();
                // config.CreateMap<Profile, ProfileCreateDto>();
                config.CreateMap<SubscriptionDto, Subscription>();
                config.CreateMap<MealPlanDto, MealPlan>();
                config.CreateMap<WorkoutHistoryDto, WorkoutHistory>();
                config.CreateMap<WorkOutDto, WorkOut>();
                // config.ReverseMap<WorkOut, WorkOutDto>(); // Ignore Id mapping for reverse maps
                // Add more mappings as needed

            return config.CreateMapper();
        }
    }
}

