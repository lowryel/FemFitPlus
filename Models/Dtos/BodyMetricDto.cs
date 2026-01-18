using System;

namespace FemFitPlus.Models.Dtos;

public class BodyMetricDto
{
    public string UserId { get; set; } = null!;
    public double WeightKg { get; set; }
    public double? HeightCm { get; set; }
    public double? BodyFatPercentage { get; set; }
    public double? MuscleMassKg { get; set; }
    public MeasurementMethod MeasurementMethod { get; set; }
    public DateTime MeasurementDate { get; set; }
}


public class UpdateBodyMetricDto
{
    public string Id { get; set; } = null!;

    public double WeightKg { get; set; }
    public double? HeightCm { get; set; }

    public double? BodyFatPercentage { get; set; }
    public double? MuscleMassKg { get; set; }

    public MeasurementMethod MeasurementMethod { get; set; }
}


public class BodyMetricProgressDto
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }

    public double? WeightChangeKg { get; set; }
    public double? BodyFatChangePercentage { get; set; }
    public double? MuscleMassChangeKg { get; set; }

    public double? BmiChange { get; set; }

    public double DaysBetween => (To - From).TotalDays;
}
