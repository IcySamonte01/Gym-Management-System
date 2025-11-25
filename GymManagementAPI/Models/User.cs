using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GymManagementAPI.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("email")]
    public string Email { get; set; } = string.Empty;

    [BsonElement("password")]
    public string? Password { get; set; }

    [BsonElement("role")]
    public string Role { get; set; } = "member";

    [BsonElement("googleId")]
    public string? GoogleId { get; set; }

    [BsonElement("profilePicture")]
    public string? ProfilePicture { get; set; }

    [BsonElement("authProvider")]
    public string AuthProvider { get; set; } = "local";

    [BsonElement("isActive")]
    public bool IsActive { get; set; } = true;

    [BsonElement("lastLogin")]
    public DateTime? LastLogin { get; set; }

    [BsonElement("createdAt")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [BsonElement("updatedAt")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // Mongoose version field - ignore this
    [BsonElement("__v")]
    [BsonIgnoreIfNull]
    public int? Version { get; set; }
}

public class UserDto
{
    public string? Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string? ProfilePicture { get; set; }
    public string AuthProvider { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime? LastLogin { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class RegisterRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? Role { get; set; }
}

public class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool RememberMe { get; set; }
}

public class LoginResponse
{
    public string Message { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public UserDto User { get; set; } = new();
}

public class CreateAdminUserRequest
{
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}

public class UpdateRoleRequest
{
    public string Role { get; set; } = string.Empty;
}
