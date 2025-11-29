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

        // Set expiration date and status based on membership type
        if (member.MembershipType.Equals("Trial", StringComparison.OrdinalIgnoreCase))
        {
            member.ExpirationDate = DateTime.UtcNow.AddDays(1);
            member.IsTrial = true;
            member.Status = "active";
        }
        else if (member.MembershipType.Equals("Monthly", StringComparison.OrdinalIgnoreCase))
        {
            member.ExpirationDate = DateTime.UtcNow.AddDays(30);
            member.Status = "active";
        }
        else if (member.MembershipType.Equals("Annual", StringComparison.OrdinalIgnoreCase))
        {
            member.ExpirationDate = DateTime.UtcNow.AddDays(365);
            member.Status = "active";
        }

        // Check if already expired
        if (member.ExpirationDate.HasValue && member.ExpirationDate.Value <= DateTime.UtcNow)
        {
            member.Status = "expired";
        }

        await _members.InsertOneAsync(member);

        // Create user account for member if password provided (not for trial)
        if (!member.IsTrial && !string.IsNullOrEmpty(member.Password))
        {
            var user = new User
            {
                Name = member.Name,
                Email = member.Email,
                Password = _authService.HashPassword(member.Password), // Hash password before storing
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
        // Fetch existing member first to preserve all fields
        var existingMember = await _members.Find(m => m.Id == id).FirstOrDefaultAsync();
        if (existingMember == null)
        {
            return null;
        }

        // Update only the fields that are provided (non-empty/non-default)
        // Always update: UpdatedAt
        existingMember.UpdatedAt = DateTime.UtcNow;
        
        // Update fields if they are provided
        if (!string.IsNullOrEmpty(member.Name))
            existingMember.Name = member.Name;
        
        if (!string.IsNullOrEmpty(member.Email))
            existingMember.Email = member.Email;
        
        if (!string.IsNullOrEmpty(member.Phone))
            existingMember.Phone = member.Phone;
        
        if (!string.IsNullOrEmpty(member.MembershipType))
            existingMember.MembershipType = member.MembershipType;
        
        if (!string.IsNullOrEmpty(member.Status))
            existingMember.Status = member.Status;
        
        // Update address if provided (can be empty string to clear it)
        if (member.Address != null)
            existingMember.Address = member.Address;
        
        // Update emergency contact if provided (can be empty string to clear it)
        if (member.EmergencyContact != null)
            existingMember.EmergencyContact = member.EmergencyContact;
        
        // Update expiration date if provided
        if (member.ExpirationDate.HasValue)
            existingMember.ExpirationDate = member.ExpirationDate;
        
        // Update trial status
        existingMember.IsTrial = member.IsTrial;
        
        // Update coach info if provided
        if (member.CoachId != null)
            existingMember.CoachId = member.CoachId;
        
        if (member.CoachName != null)
            existingMember.CoachName = member.CoachName;
        
        // Update student status
        existingMember.IsStudent = member.IsStudent;
        
        // Update password only if provided
        if (!string.IsNullOrEmpty(member.Password))
            existingMember.Password = member.Password;
        
        // Update age if provided (greater than 0)
        if (member.Age > 0)
            existingMember.Age = member.Age;
        
        await _members.ReplaceOneAsync(m => m.Id == id, existingMember);
        return existingMember;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _members.DeleteOneAsync(m => m.Id == id);
        return result.DeletedCount > 0;
    }
}
