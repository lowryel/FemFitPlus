using System;

namespace FemFitPlus.Models.Dtos;

public class FemFitUserDto
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Token { get; set; }
    public string? IsSubscribed { get; set; }
    public string? IsActive { get; set; }
}
