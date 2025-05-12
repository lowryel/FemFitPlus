using FemFitPlus.Models;
using FemFitPlus.Models.Dtos;
using FemFitPlus.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Mlapper.Auto.Mapper;
using System.Threading.Tasks;

namespace FemFitPlus.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(
        UserManager<FemFitUser> userManager,
        IUserAuthService service, IMapper mapper) : ControllerBase
    {
        private readonly UserManager<FemFitUser> _userManager = userManager;

        private readonly IUserAuthService _service = service;
        private readonly IMapper _mapper = mapper;


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            try
            {
                var user = await _service.LoginAsync(model);
                if (user == null)
                {
                    return NotFound("User not found");
                }
                var login = _mapper.Map<FemFitUserDto, FemFitUser>(user);
                return Ok(new FemFitUserDto
                {
                    Id = login.Id,
                    FirstName = login.FirstName,
                    LastName = login.LastName,
                    Email = login.Email,
                    Token = user.Token,
                    DateOfBirth = login.DateOfBirth
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid registration data");
                }
                var user = await _service.RegisterAsync(model);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }

        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value
                    ?? throw new ArgumentException("User ID cannot be null or empty");
                var user = await _service.GetProfile(userId);
                if (user == null)
                {
                    return NotFound();
                }
                // Map the user to a DTO

                return Ok(new
                {
                    Id = user.Id,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    IsSubscribed = user.IsSubscribed
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Change password
        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
        {
            try
            {
                var userId = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var user = await _service.ChangePasswordAsync(model, userId);
                if (user == false)
                {
                    return BadRequest("Password change failed");
                }
                return Ok(new { Message = "Password changed successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // Forgot password
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordDto model)
        {
            try
            {
                await _service.ForgotPasswordAsync(model);
                return Ok(new { message = "If your email is registered, you will receive a password reset link." });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while processing your request." });
            }
        }

        // Reset password
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            try
            {
                var user = await _service.ResetPasswordAsync(model);
                if (user == false)
                {
                    return BadRequest("Password reset failed");
                }
                // Send email with reset link
                return Ok(new { Message = "Password has been reset successfully" });
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        // Update profile
        [Authorize]
        [HttpPut("update-profile")]
        public async Task<IActionResult> UpdateProfile(UpdateProfileDto model)
        {
            try
            {
                var userId = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                var user = await _service.UpdateProfileAsync(model, userId);
                if (user == false)
                {
                    return BadRequest("Profile update failed");
                }

                return Ok(new { Message = "Profile updated successfully" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return BadRequest(new { message = "An error occurred while processing your request." });
            }
        }

        // Confirm email
        [HttpGet("confirm-email")]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                var confirmMail = await _service.ConfirmEmail(userId, token);

                if (confirmMail == false)
                {
                    return BadRequest("Email confirmation failed");
                }
                return Ok(new { Message = "Email confirmed successfully" });
            }
            catch (Exception)
            {
                throw new ArgumentException("Invalid token");
            }
        }

        // Get user roles
        [Authorize(Roles = "Admin")]
        [HttpGet("user-roles/{userId}")]
        public async Task<IActionResult> GetUserRoles([FromRoute] string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { UserId = userId, Roles = roles });
        }

        // Assign role to user
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole(AssignRoleDto model)
        {
            var role = await _service.AssignRoleAsync(model);
            if (role == false)
            {
                return BadRequest("Role assignment failed");
            }

            return Ok(new { Message = $"Role {model.RoleName} assigned to user successfully" });
        }

        // Remove role from user
        [Authorize(Roles = "Admin")]
        [HttpPost("remove-role")]
        public async Task<IActionResult> RemoveRole(AssignRoleDto model)
        {
            try
            {
                var userId = User.FindFirst(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                model.UserId = userId;
                var role = await _service.RemoveRole(model);
                if (role == false)
                {
                    return BadRequest("Role removal failed");
                }
                return Ok(new { Message = $"Role {model.RoleName} removed from user successfully" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // List all users (admin only)
        [Authorize(Roles = "Admin")]
        [HttpGet("list")]
        public async Task<IActionResult> ListUsers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = _userManager.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(u => new
                {
                    u.Id,
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.IsActive,
                    u.EmailConfirmed
                })
                .ToList();

            var totalUsers = await Task.FromResult(_userManager.Users.Count());

            return Ok(new
            {
                Users = users,
                TotalCount = totalUsers,
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalUsers / (double)pageSize)
            });
        }

        // Lock user account
        [Authorize(Roles = "Admin")]
        [HttpPost("lock-account/{userId}")]
        public async Task<IActionResult> LockAccount([FromRoute] string userId, [FromQuery] int days = 14)
        {
            try
            {
                var account = await _service.LockAccount(userId, days);
                if (account == null)
                {
                    return BadRequest("Account locking failed");
                }
                return Ok(new { Message = $"User account locked for {days} days" });
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Unlock user account
        [Authorize(Roles = "Admin")]
        [HttpPost("unlock-account/{userId}")]
        public async Task<IActionResult> UnlockAccount(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }

            var result = await _userManager.SetLockoutEndDateAsync(user, null);
            if (!result.Succeeded)
            {
                return BadRequest(new { Errors = result.Errors });
            }

            return Ok(new { Message = "User account unlocked successfully" });
        }

    }
}
