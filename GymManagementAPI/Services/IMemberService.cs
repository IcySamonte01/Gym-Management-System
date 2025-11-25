using GymManagementAPI.Models;

namespace GymManagementAPI.Services;

public interface IMemberService
{
    Task<List<Member>> GetAllAsync();
    Task<Member?> GetByIdAsync(string id);
    Task<Member> CreateAsync(Member member);
    Task<Member?> UpdateAsync(string id, Member member);
    Task<bool> DeleteAsync(string id);
}
