using System;

namespace FemFitPlus.Models.Dtos;

public class MealPlanDto
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public string? MealPlanId { get; set; }
    public DateTime Date { get; set; }
}
