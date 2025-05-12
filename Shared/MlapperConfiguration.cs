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
                config.CreateMap<CycleDto, Cycle>();
                config.CreateMap<CycleCreateDto, Cycle>();
                config.CreateMap<CycleUpdateDto, Cycle>();
                config.CreateMap<ProfileCreateDto, Profile>();
                config.CreateMap<ProfileDto, Profile>();
                config.CreateMap<Profile, ProfileCreateDto>();
                config.CreateMap<SubscriptionDto, Subscription>();
                config.CreateMap<MealPlanDto, MealPlan>();
                config.CreateMap<WorkoutHistoryDto, WorkoutHistory>();
                config.CreateMap<WorkOutDto, WorkOut>();
                // Add more mappings as needed

            return config.CreateMapper();
        }
    }
}

