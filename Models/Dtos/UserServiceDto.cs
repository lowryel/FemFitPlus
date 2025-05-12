using System;

namespace FemFitPlus.Models.Dtos;

public class UserServiceDto
{
}
public class LoginDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
}

public class RegisterDto
{
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
}


// DTOs for requests
public class ChangePasswordDto
{
    public string? CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}

public class ForgotPasswordDto
{
    public string? Email { get; set; }
}

public class ResetPasswordDto
{
    public string? Email { get; set; }
    public string? Token { get; set; }
    public string? Password { get; set; }
}

public class UpdateProfileDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
}

public class AssignRoleDto
{
    public string? UserId { get; set; }
    public string? RoleName { get; set; }
}

