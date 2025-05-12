using System;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class MealPlan: AuditFields
{
    public string? PhaseTargeted { get; set; }
    public string? MealType { get; set; }
    public int Calories { get; set; }
    public string? Macros { get; set; }
    public string? RecipeLink { get; set; }
    public string? Notes { get; set; }
}
