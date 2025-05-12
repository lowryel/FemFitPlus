using System;

namespace FemFitPlus.Models.Dtos;

public class MealPlanCreateDto
{
    public string? UserId { get; set; }
    public string? MealPlanId { get; set; }
    public DateTime Date { get; set; }
}
