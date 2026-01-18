using System;
using FemFitPlus.Data;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services.Filters;
using FemFitPlus.Shared;
using Microsoft.EntityFrameworkCore;

namespace FemFitPlus.Services;

public class BodyMetricService(FemFitPlusContext context) : IBodyMetricService
{

    private readonly FemFitPlusContext _context = context;
    public async Task<string?> Create(BodyMetricDto dto)
    {
        if (dto.WeightKg <= 0 || dto.WeightKg > 400)
            throw new ArgumentException("Invalid weight value.");

        if (dto.BodyFatPercentage is < 2 or > 70)
            throw new ArgumentException("Invalid body fat percentage.");

        var bodyMetric = new BodyMetric
        {
            FemFitUserId = dto.UserId,

            WeightKg = dto.WeightKg,
            HeightCm = dto.HeightCm,
            BodyFatPercentage = dto.BodyFatPercentage,
            MuscleMassKg = dto.MuscleMassKg,

            BMI = Helpers.CalculateBmi(dto.WeightKg, dto.HeightCm),

            MeasurementMethod = dto.MeasurementMethod,
            RecordedAt = dto.MeasurementDate
        };

        // _context.BodyMetrics.Add(bodyMetric);
        _context.Entry(bodyMetric).State = EntityState.Added; // similar to the line above
        await _context.SaveChangesAsync();

        return bodyMetric.Id;
    }

    public Task<bool> Delete(string bodyMetricId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<BodyMetricDto> Get(string bodyMetricId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BodyMetricDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<List<BodyMetricDto>> GetByUserId(string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BodyMetricDto>> Query(BodyMetricFilter filter)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> Update(UpdateBodyMetricDto dto)
    {
        var bodyMetric = await _context.BodyMetrics
        .FirstOrDefaultAsync(x => x.Id == dto.Id);
        if (bodyMetric == null)
            return false;

        if ((DateTime.UtcNow - bodyMetric!.RecordedAt).TotalDays > 7)
            throw new InvalidOperationException("Metrics older than 7 days cannot be edited.");

        if (Math.Abs(bodyMetric.WeightKg - dto.WeightKg) > 20)
            throw new InvalidOperationException("Unrealistic weight change detected.");

        // Update allowed fields
        bodyMetric.WeightKg = dto.WeightKg;
        bodyMetric.HeightCm = dto.HeightCm;
        bodyMetric.BodyFatPercentage = dto.BodyFatPercentage;
        bodyMetric.MuscleMassKg = dto.MuscleMassKg;
        bodyMetric.MeasurementMethod = dto.MeasurementMethod;

        // Recalculate derived values
        bodyMetric.BMI = Helpers.CalculateBmi(dto.WeightKg, dto.HeightCm);

        // Audit field (assuming AuditFields has this)
        bodyMetric.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<BodyMetricProgressDto?> GetWeeklyProgressAsync(
    string userId,
    bool isPregnant)
    {
        var end = DateTime.UtcNow;
        var start = end.AddDays(-7);

        var metrics = await GetMetricsAsync(userId, start, end);

        if (metrics.Count < 2)
            return null;

        return CalculateProgress(metrics.First(), metrics.Last(), isPregnant);
    }

    public async Task<BodyMetricProgressDto?> GetMonthlyProgressAsync(
    string userId,
    bool isPregnant)
    {
        var end = DateTime.UtcNow;
        var start = end.AddDays(-30);

        var metrics = await GetMetricsAsync(userId, start, end);

        if (metrics.Count < 2)
            return null;

        return CalculateProgress(metrics.First(), metrics.Last(), isPregnant);
    }



    private async Task<List<BodyMetric>> GetMetricsAsync(
    string userId,
    DateTime from,
    DateTime to)
    {
        return await _context.BodyMetrics
            .Where(x =>
                x.FemFitUserId == userId &&
                x.RecordedAt >= from &&
                x.RecordedAt <= to)
            .OrderBy(x => x.RecordedAt)
            .ToListAsync();
    }

    private static BodyMetricProgressDto? CalculateProgress(
    BodyMetric start,
    BodyMetric end,
    bool isPregnant)
    {
        return new BodyMetricProgressDto
        {
            From = start.RecordedAt,
            To = end.RecordedAt,

            WeightChangeKg = end.WeightKg - start.WeightKg,

            BodyFatChangePercentage =
                start.BodyFatPercentage.HasValue && end.BodyFatPercentage.HasValue
                    ? end.BodyFatPercentage - start.BodyFatPercentage
                    : null,

            MuscleMassChangeKg =
                start.MuscleMassKg.HasValue && end.MuscleMassKg.HasValue
                    ? end.MuscleMassKg - start.MuscleMassKg
                    : null,

            // BMI only if not pregnant
            BmiChange =
                !isPregnant &&
                start.BMI.HasValue &&
                end.BMI.HasValue
                    ? end.BMI - start.BMI
                    : null
        };
    }

}
