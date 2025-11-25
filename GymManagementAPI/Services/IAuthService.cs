using GymManagementAPI.Models;

namespace GymManagementAPI.Services;

public interface IAuthService
{
    string GenerateJwtToken(User user);
    string HashPassword(string password);
    bool VerifyPassword(string password, string hashedPassword);
}
