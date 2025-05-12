using System;
using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Mlapper.Auto.Mapper;

namespace FemFitPlus.Services;

public class UserAuthService(UserManager<FemFitUser> userManager,
        RoleManager<IdentityRole> roleManager,
        SignInManager<FemFitUser> signInManager,
        // IEmailSender emailSender,
        IJwtTokenService tokenService, IMapper mapper) : IUserAuthService
{
    private readonly UserManager<FemFitUser> _userManager = userManager;
    private readonly SignInManager<FemFitUser> _signInManager = signInManager;
    private readonly IJwtTokenService _tokenService = tokenService;
    private readonly RoleManager<IdentityRole> _roleManager = roleManager;
    private readonly IMapper _mapper = mapper;
    // private readonly IEmailSender _emailSender= emailSender;


    public async Task<bool> ChangePasswordAsync(ChangePasswordDto changePasswordDto, string? userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty");
            }

            if (changePasswordDto == null || string.IsNullOrEmpty(changePasswordDto.CurrentPassword) || string.IsNullOrEmpty(changePasswordDto.NewPassword))
            {
                throw new ArgumentException("Invalid password change request");
            }
            var user = await _userManager.FindByIdAsync(userId!);

            if (user == null)
            {
                throw new NullReferenceException("User not found");
            }

            var result = await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword!, changePasswordDto.NewPassword!);

            if (!result.Succeeded)
            {
                throw new Exception("Password change failed");
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<bool> ForgotPasswordAsync(ForgotPasswordDto forgotPasswordDto)
    {
        try
        {
            if (forgotPasswordDto == null || string.IsNullOrEmpty(forgotPasswordDto.Email))
            {
                throw new ArgumentException("Invalid email address");
            }

            if (forgotPasswordDto == null || string.IsNullOrEmpty(forgotPasswordDto.Email))
                throw new ArgumentException("Invalid email address");

            var user = await _userManager.FindByEmailAsync(forgotPasswordDto.Email!);

            if (user == null)
            {
                // Don't reveal user existence
                return true;
            }

            // var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // var callbackUrl = $"https://yourfrontend.com/reset-password?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(user.Email!)}";

            // // Send the email (optional)
            // await _emailSender.SendEmailAsync(
            //     user.Email!,
            //     "Reset Password",
            //     $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");

            return true;


        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing your request", ex);
        }
        throw new NotImplementedException();
    }

    public async Task<FemFitUserDto> GetProfile(string userId)
    {
        try
        {
            // var userId = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty");
            }

            var user = await _userManager.FindByIdAsync(userId) ?? throw new NullReferenceException("User not found");

            return new FemFitUserDto
            {
                Id = user.Id,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = await _tokenService.GenerateJwtTokenAsync(user),
                DateOfBirth = user.DateOfBirth,
                IsSubscribed = user.IsSubscribed.ToString(),
                IsActive = user.IsActive.ToString(),
            };
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while retrieving the profile", ex);
        }
        throw new NotImplementedException();
    }

    public async Task<FemFitUserDto> LoginAsync(LoginDto loginDto)
    {
        try
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                throw new ArgumentException("Invalid login credentials");
            }
            var user = await _userManager.FindByEmailAsync(loginDto.Email!) ?? throw new NullReferenceException("User not found");
            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password!, false);
            if (!result.Succeeded)
            {
                throw new NullReferenceException("Incorrect password");
            }

            var token = await _tokenService.GenerateJwtTokenAsync(user);

            return new FemFitUserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Token = token,
                DateOfBirth = user.DateOfBirth,
            };
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Incorrect password entered!", ex);
        }
    }

    public async Task<RegisterDto> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            if (registerDto == null || string.IsNullOrEmpty(registerDto.Email) || string.IsNullOrEmpty(registerDto.Password))
            {
                throw new ArgumentException("Invalid registration details");
            }

            var user = new FemFitUser
            {
                UserName = registerDto.Email,
                Email = registerDto.Email,
                FirstName = registerDto.FirstName!,
                LastName = registerDto.LastName!,
                DateOfBirth = registerDto.DateOfBirth,
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password!);
            if (!result.Succeeded)
            {
                throw new Exception("User registration failed");
            }

            return new RegisterDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                DateOfBirth = user.DateOfBirth,
            };
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing your request", ex);
        }
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordDto resetPasswordDto)
    {
        try
        {
            if (resetPasswordDto == null || string.IsNullOrEmpty(resetPasswordDto.Email) || string.IsNullOrEmpty(resetPasswordDto.Token) || string.IsNullOrEmpty(resetPasswordDto.Password))
            {
                throw new ArgumentException("Invalid password reset request");
            }

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email!);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                throw new NullReferenceException("User not found");
            }

            var token = await _userManager.VerifyUserTokenAsync(user, "Default", "ResetPassword", resetPasswordDto.Token!);
            if (!token)
            {
                throw new Exception("Invalid token");
            }
            var result = await _userManager.ResetPasswordAsync(user, resetPasswordDto.Token!, resetPasswordDto.Password!);
            if (!result.Succeeded)
            {
                throw new Exception("Password reset failed");
            }
            return true;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing your request", ex);
        }
    }

    public async Task<bool> UpdateProfileAsync(UpdateProfileDto updateProfileDto, string? userId)
    {
        try
        {
            // var userId = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentException("User ID cannot be null or empty");
            }

            if (updateProfileDto == null || string.IsNullOrEmpty(updateProfileDto.Email))
            {
                throw new ArgumentException("Invalid profile update request");
            }

            var user = await _userManager.FindByIdAsync(userId!) ?? throw new NullReferenceException("User not found");
            user.FirstName = updateProfileDto.FirstName!;
            user.LastName = updateProfileDto.LastName!;
            user.Email = updateProfileDto.Email!;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new Exception("Profile update failed");
            }

            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public async Task<bool> AssignRoleAsync(AssignRoleDto assignRoleDto)
    {
        try
        {
            if (assignRoleDto == null || string.IsNullOrEmpty(assignRoleDto.UserId) || string.IsNullOrEmpty(assignRoleDto.RoleName))
            {
                throw new ArgumentException("Invalid role assignment request");
            }

            // Verify role exists
            var roleExists = await _roleManager.RoleExistsAsync(assignRoleDto.RoleName!);
            if (!roleExists)
            {
                throw new ArgumentException("Role does not exist");
            }

            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId!);
            if (user == null)
            {
                throw new NullReferenceException("User not found");
            }

            var result = await _userManager.AddToRoleAsync(user, assignRoleDto.RoleName!);
            if (!result.Succeeded)
            {
                throw new Exception("Role assignment failed");
            }
            return true;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing your request", ex);
        }
    }

    public async Task<bool> ConfirmEmail(string userId, string token)
    {
        try
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
            {
                throw new ArgumentException("Invalid email confirmation request");
            }

            var user = await _userManager.FindByIdAsync(userId!) ?? throw new NullReferenceException("User not found");
            var result = await _userManager.ConfirmEmailAsync(user, token!);
            if (!result.Succeeded)
            {
                throw new Exception("Email confirmation failed");
            }
            return true;
        }
        catch (ArgumentException ex)
        {
            throw new ArgumentException(ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while processing your request", ex);
        }
    }

    public async Task<bool> RemoveRole(AssignRoleDto assignRoleDto)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(assignRoleDto.UserId!) ?? throw new NullReferenceException("User not found");
            var result = await _userManager.RemoveFromRoleAsync(user, assignRoleDto.RoleName!);
            if (!result.Succeeded)
            {
                throw new Exception("Role removal failed");
            }
            return true;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    public async Task<string> LockAccount(string userId, int days)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(userId) ?? throw new NullReferenceException("User not found");
            var result = await _userManager.SetLockoutEndDateAsync(user, DateTime.UtcNow.AddDays(days));
            if (!result.Succeeded)
            {
                throw new Exception("Account locking failed");
            }
            return "Account locked successfully";
        }
        catch (System.Exception)
        {
            throw;
        }
    }
}
