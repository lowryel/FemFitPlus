using System;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;

namespace FemFitPlus.Services;

public interface IBodyMetricService
{
    Task<string?> Create(BodyMetricDto bodyMetricDto);
    Task<BodyMetricDto> Get(string bodyMetricId);
    Task<bool> Update(UpdateBodyMetricDto dto);
    Task<bool> Delete(string bodyMetricId, string userId);
    Task<List<BodyMetricDto>> GetAll();
    Task<List<BodyMetricDto>> GetByUserId(string userId);
    Task<List<BodyMetricDto>> Query(BodyMetricFilter filter);
    Task<BodyMetricProgressDto?> GetWeeklyProgressAsync(
        string userId,
        bool isPregnant);

    Task<BodyMetricProgressDto?> GetMonthlyProgressAsync(
        string userId,
        bool isPregnant);
}
