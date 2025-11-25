using GymManagementAPI.Models;
using GymManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/admin")]
[Authorize(Roles = "admin")]
public class AdminController : ControllerBase
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers()
    {
        try
        {
            var users = await _userService.GetAllAsync();
            
            var userDtos = users.Select(u => new UserDto
            {
                Id = u.Id,
                Name = u.Name,
                Email = u.Email,
                Role = u.Role,
                ProfilePicture = u.ProfilePicture,
                AuthProvider = u.AuthProvider,
                IsActive = u.IsActive,
                LastLogin = u.LastLogin,
                CreatedAt = u.CreatedAt
            }).ToList();

            return Ok(userDtos);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateAdminUserRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || 
                string.IsNullOrEmpty(request.Password) || string.IsNullOrEmpty(request.Role))
            {
                return BadRequest(new { error = "Please provide all required fields" });
            }

            // Check if user already exists
            var existingUser = await _userService.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { error = "User with this email already exists" });
            }

            // Validate role
            if (!new[] { "member", "coach", "admin" }.Contains(request.Role))
            {
                return BadRequest(new { error = "Invalid role" });
            }

            // Create new user (including admin)
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role,
                AuthProvider = "local"
            };

            var createdUser = await _userService.CreateAsync(user);

            return StatusCode(201, new
            {
                message = "User created successfully",
                user = new UserDto
                {
                    Id = createdUser.Id,
                    Name = createdUser.Name,
                    Email = createdUser.Email,
                    Role = createdUser.Role
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPatch("users/{id}/role")]
    public async Task<IActionResult> UpdateUserRole(string id, [FromBody] UpdateRoleRequest request)
    {
        try
        {
            // Validate role
            if (!new[] { "member", "coach", "admin" }.Contains(request.Role))
            {
                return BadRequest(new { error = "Invalid role" });
            }

            var user = await _userService.UpdateRoleAsync(id, request.Role);

            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new
            {
                message = "User role updated successfully",
                user = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPatch("users/{id}/deactivate")]
    public async Task<IActionResult> DeactivateUser(string id)
    {
        try
        {
            var currentUserId = User.FindFirst("id")?.Value;
            
            // Prevent admin from deactivating themselves
            if (id == currentUserId)
            {
                return BadRequest(new { error = "Cannot deactivate your own account" });
            }

            var user = await _userService.UpdateActiveStatusAsync(id, false);

            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new
            {
                message = "User deactivated successfully",
                user = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IsActive = user.IsActive
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPatch("users/{id}/activate")]
    public async Task<IActionResult> ActivateUser(string id)
    {
        try
        {
            var user = await _userService.UpdateActiveStatusAsync(id, true);

            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new
            {
                message = "User activated successfully",
                user = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    IsActive = user.IsActive
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var currentUserId = User.FindFirst("id")?.Value;
            
            // Prevent admin from deleting themselves
            if (id == currentUserId)
            {
                return BadRequest(new { error = "Cannot delete your own account" });
            }

            var deleted = await _userService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new { message = "User deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
