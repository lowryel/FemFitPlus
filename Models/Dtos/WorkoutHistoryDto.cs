using System;

namespace FemFitPlus.Models.Dtos;

public class WorkoutHistoryDto
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? WorkOutId { get; set; }
    public DateTime Date { get; set; }
}


public class WorkoutHistoryCreateDto
{
    public string? UserId { get; set; }
    public string? WorkOutId { get; set; }
    public DateTime Date { get; set; }
}