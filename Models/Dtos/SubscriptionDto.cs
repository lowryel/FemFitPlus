using System;

namespace FemFitPlus.Models.Dtos;

public class SubscriptionDto
{
    public string UserId { get; set; } = null!;
    public string? StripeCustomerId { get; set; }
    public string? PlanName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }

    public FemFitUser? Femfituser { get; set; }
}
