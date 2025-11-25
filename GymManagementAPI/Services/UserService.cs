using GymManagementAPI.Models;
using MongoDB.Driver;

namespace GymManagementAPI.Services;

public class UserService : IUserService
{
    private readonly IMongoCollection<User> _users;
    private readonly IAuthService _authService;

    public UserService(IMongoDatabase database, IAuthService authService)
    {
        _users = database.GetCollection<User>("users");
        _authService = authService;
    }

    public async Task<User?> GetByIdAsync(string id)
    {
        return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _users.Find(u => u.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User?> GetByGoogleIdAsync(string googleId)
    {
        return await _users.Find(u => u.GoogleId == googleId).FirstOrDefaultAsync();
    }

    public async Task<List<User>> GetAllAsync()
    {
        return await _users.Find(_ => true)
            .SortByDescending(u => u.CreatedAt)
            .ToListAsync();
    }

    public async Task<User> CreateAsync(User user)
    {
        // Hash password if provided
        if (!string.IsNullOrEmpty(user.Password))
        {
            user.Password = _authService.HashPassword(user.Password);
        }

        user.CreatedAt = DateTime.UtcNow;
        user.UpdatedAt = DateTime.UtcNow;

        await _users.InsertOneAsync(user);
        return user;
    }

    public async Task<User?> UpdateAsync(string id, User user)
    {
        user.UpdatedAt = DateTime.UtcNow;
        user.Id = id;
        
        await _users.ReplaceOneAsync(u => u.Id == id, user);
        return user;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _users.DeleteOneAsync(u => u.Id == id);
        return result.DeletedCount > 0;
    }

    public async Task<User?> UpdateRoleAsync(string id, string role)
    {
        var update = Builders<User>.Update
            .Set(u => u.Role, role)
            .Set(u => u.UpdatedAt, DateTime.UtcNow);

        await _users.UpdateOneAsync(u => u.Id == id, update);
        return await GetByIdAsync(id);
    }

    public async Task<User?> UpdateActiveStatusAsync(string id, bool isActive)
    {
        var update = Builders<User>.Update
            .Set(u => u.IsActive, isActive)
            .Set(u => u.UpdatedAt, DateTime.UtcNow);

        await _users.UpdateOneAsync(u => u.Id == id, update);
        return await GetByIdAsync(id);
    }
}
