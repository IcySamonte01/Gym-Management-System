using GymManagementAPI.Models;
using MongoDB.Driver;

namespace GymManagementAPI.Services;

public class MemberService : IMemberService
{
    private readonly IMongoCollection<Member> _members;
    private readonly IMongoCollection<User> _users;
    private readonly IAuthService _authService;

    public MemberService(IMongoDatabase database, IAuthService authService)
    {
        _members = database.GetCollection<Member>("members");
        _users = database.GetCollection<User>("users");
        _authService = authService;
    }

    public async Task<List<Member>> GetAllAsync()
    {
        return await _members.Find(_ => true).ToListAsync();
    }

    public async Task<Member?> GetByIdAsync(string id)
    {
        return await _members.Find(m => m.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Member> CreateAsync(Member member)
    {
        member.CreatedAt = DateTime.UtcNow;
        member.UpdatedAt = DateTime.UtcNow;
        await _members.InsertOneAsync(member);

        // Create user account for member if password provided
        if (!string.IsNullOrEmpty(member.Password))
        {
            var user = new User
            {
                Name = member.Name,
                Email = member.Email,
                Password = member.Password,
                Role = "member",
                AuthProvider = "local",
                IsActive = true
            };
            await _users.InsertOneAsync(user);
        }

        return member;
    }

    public async Task<Member?> UpdateAsync(string id, Member member)
    {
        member.UpdatedAt = DateTime.UtcNow;
        member.Id = id;
        
        await _members.ReplaceOneAsync(m => m.Id == id, member);
        return member;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _members.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}
