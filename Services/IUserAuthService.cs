using System;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;

namespace FemFitPlus.Services;

public interface IUserAuthService
{
    Task<FemFitUserDto> LoginAsync(LoginDto loginDto);
    Task<RegisterDto> RegisterAsync(RegisterDto registerDto);
    Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, string? userId);
    Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto);
    Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto);
    Task<bool> UpdateProfileAsync(UpdateProfileDto updateProfileDto, string? userId);
    Task<bool> AssignRoleAsync(AssignRoleDto assignRoleDto);
    Task<FemFitUserDto> GetProfile(string userId);
    Task<bool> ConfirmEmail(string userId, string token);
    Task<bool> RemoveRole(AssignRoleDto assignRoleDto);
    Task<string> LockAccount(string userId,int days);
}
