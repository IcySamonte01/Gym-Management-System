using GymManagementAPI.Models;
using GymManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;

    public AuthController(IUserService userService, IAuthService authService)
    {
        _userService = userService;
        _authService = authService;
    }

    [HttpPost("register")]
    [Authorize(Roles = "admin")] // REMOVED PUBLIC ACCESS - Only admin can create accounts
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrEmpty(request.Name) || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Please provide all required fields" });
            }

            // Check if user already exists
            var existingUser = await _userService.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return BadRequest(new { error = "User with this email already exists" });
            }

            var userRole = request.Role ?? "member";

            // Create new user (admin can create any role)
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password,
                Role = userRole,
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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        try
        {
            // Validate input
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest(new { error = "Please provide email and password" });
            }

            // Find user
            var user = await _userService.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            // Check if user is active
            if (!user.IsActive)
            {
                return Unauthorized(new { error = "Account is deactivated. Please contact support." });
            }

            // Check password
            if (string.IsNullOrEmpty(user.Password) || !_authService.VerifyPassword(request.Password, user.Password))
            {
                return Unauthorized(new { error = "Invalid email or password" });
            }

            // Update last login
            user.LastLogin = DateTime.UtcNow;
            await _userService.UpdateAsync(user.Id!, user);

            // Generate JWT token
            var token = _authService.GenerateJwtToken(user);

            return Ok(new LoginResponse
            {
                Message = "Login successful",
                Token = token,
                User = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePicture = user.ProfilePicture
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("verify")]
    [Authorize]
    public async Task<IActionResult> Verify()
    {
        try
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { error = "Invalid token" });
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user == null || !user.IsActive)
            {
                return Unauthorized(new { error = "Invalid token or user deactivated" });
            }

            return Ok(new
            {
                valid = true,
                user = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePicture = user.ProfilePicture
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost("logout")]
    [Authorize]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logout successful" });
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<IActionResult> GetProfile()
    {
        try
        {
            var userId = User.FindFirst("id")?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new { error = "Invalid token" });
            }

            var user = await _userService.GetByIdAsync(userId);
            if (user == null)
            {
                return NotFound(new { error = "User not found" });
            }

            return Ok(new
            {
                user = new UserDto
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Role = user.Role,
                    ProfilePicture = user.ProfilePicture,
                    AuthProvider = user.AuthProvider,
                    CreatedAt = user.CreatedAt
                }
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
