using System;
using FemFitPlus.Shared;
using Microsoft.AspNetCore.Identity;

namespace FemFitPlus.Models;

public class FemFitUser: IdentityUser
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public DateTime DateOfBirth { get; set; }
    public bool IsActive { get; set; }
    public bool IsSubscribed { get; set; }
    public Profile? Profile { get; set; }
    public List<Subscription>? Subscriptions { get; set; }
    public List<Consultation>? Consultations { get; set; }
    public List<WorkoutHistory>? WorkoutHistory { get; set; }
    public List<Cycle>? Cycles { get; set; }
    public List<BodyMetric>? BodyMetrics { get; set; }
}
