using System;
using System.ComponentModel.DataAnnotations.Schema;
using FemFitPlus.Shared;

namespace FemFitPlus.Models;

public class Consultation: AuditFields
{
    public string CoachId { get; set; } = null!;
    [ForeignKey("UserId")]
    public FemFitUser? Coach { get; set; }
    public DateTime Startime { get; set; }
    public DateTime EndTime { get; set; }
    public string? MeetingLink { get; set; }    
    public Status Status { get; set; }
    public string? Notes { get; set; }
    public string? Feedback { get; set; }
    public string? FollowUp { get; set; }
    public string? FollowUpLink { get; set; }
}


public enum Status
{
    Scheduled,
    Completed,
    Cancelled
}