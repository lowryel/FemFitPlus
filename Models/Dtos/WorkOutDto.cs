using System;

namespace FemFitPlus.Models.Dtos;

public class WorkOutDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public DateTime Date { get; set; }
    public int CycleId { get; set; }
    public int ProfileId { get; set; }
    public int ConsultantId { get; set; }
}


public class WorkOutCreateDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Duration { get; set; }
    public DateTime Date { get; set; }
    public int CycleId { get; set; }
    public int ProfileId { get; set; }
    public int ConsultantId { get; set; }
}
