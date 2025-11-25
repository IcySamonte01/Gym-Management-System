using GymManagementAPI.Models;

namespace GymManagementAPI.Services;

public interface IUserService
{
    Task<User?> GetByIdAsync(string id);
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByGoogleIdAsync(string googleId);
    Task<List<User>> GetAllAsync();
    Task<User> CreateAsync(User user);
    Task<User?> UpdateAsync(string id, User user);
    Task<bool> DeleteAsync(string id);
    Task<User?> UpdateRoleAsync(string id, string role);
    Task<User?> UpdateActiveStatusAsync(string id, bool isActive);
}
