using System;

namespace FemFitPlus.Models.Dtos;

public class ConsultationDto
{
    public Guid CoachId { get; set; }
    public string UserId { get; set; } = null!;
    public FemFitUser? Femfituser { get; set; }
    public DateTime Startime { get; set; }
    public DateTime EndTime { get; set; }
    public int MeetingLink { get; set; }
    public Status Status { get; set; }
    public string? Notes { get; set; }
    public string? Feedback { get; set; }
    public string? FollowUp { get; set; }
    public string? FollowUpLink { get; set; }
}
