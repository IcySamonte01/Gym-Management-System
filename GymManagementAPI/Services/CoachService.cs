using GymManagementAPI.Models;
using MongoDB.Driver;

namespace GymManagementAPI.Services;

public class CoachService : ICoachService
{
    private readonly IMongoCollection<Coach> _coaches;
    private readonly IMongoCollection<User> _users;
    private readonly IAuthService _authService;

    public CoachService(IMongoDatabase database, IAuthService authService)
    {
        _coaches = database.GetCollection<Coach>("coaches");
        _users = database.GetCollection<User>("users");
        _authService = authService;
    }

    public async Task<List<Coach>> GetAllAsync()
    {
        return await _coaches.Find(_ => true).ToListAsync();
    }

    public async Task<Coach?> GetByIdAsync(string id)
    {
        return await _coaches.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Coach> CreateAsync(Coach coach)
    {
        coach.CreatedAt = DateTime.UtcNow;
        coach.UpdatedAt = DateTime.UtcNow;
        await _coaches.InsertOneAsync(coach);

        // Create user account for coach if password provided
        if (!string.IsNullOrEmpty(coach.Password))
        {
            var user = new User
            {
                Name = coach.Name,
                Email = coach.Email,
                Password = coach.Password,
                Role = "coach",
                AuthProvider = "local",
                IsActive = true
            };
            await _users.InsertOneAsync(user);
        }

        return coach;
    }

    public async Task<Coach?> UpdateAsync(string id, Coach coach)
    {
        coach.UpdatedAt = DateTime.UtcNow;
        coach.Id = id;
        
        await _coaches.ReplaceOneAsync(c => c.Id == id, coach);
        return coach;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _coaches.DeleteOneAsync(c => c.Id == id);
        return result.DeletedCount > 0;
    }
}
