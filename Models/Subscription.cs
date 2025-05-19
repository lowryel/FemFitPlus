using System;
using System.ComponentModel.DataAnnotations.Schema;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class Subscription: AuditFields
{
    public string UserId { get; set; } = null!;
    public string? StripeCustomerId { get; set; }
    public string? PlanName { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; }
    [ForeignKey("UserId")]
    public FemFitUser? Femfituser { get; set; }
}
