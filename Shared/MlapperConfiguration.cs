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
                config.CreateMap<ConsultationDto, Consultation>().ReverseMap();
                config.CreateMap<FemFitUserDto, FemFitUser>();
                config.CreateMap<CycleDto, Cycle>().ReverseMap();
                config.CreateMap<CycleCreateDto, Cycle>().ReverseMap();
                config.CreateMap<CycleUpdateDto, Cycle>().ReverseMap();
                config.CreateMap<ProfileCreateDto, Profile>().ReverseMap();
                config.CreateMap<ProfileDto, Profile>().ReverseMap();
                config.CreateMap<SubscriptionDto, Subscription>().ReverseMap();
                config.CreateMap<MealPlanDto, MealPlan>().ReverseMap();
                config.CreateMap<WorkoutHistoryDto, WorkoutHistory>().ReverseMap();
                config.CreateMap<WorkOutDto, WorkOut>().ReverseMap();
                config.CreateMap<BodyMetricDto, BodyMetric>().ReverseMap();

            return config.CreateMapper();
        }
    }
}

