using GymManagementAPI.Models;

namespace GymManagementAPI.Services;

public interface IScheduleService
{
    Task<List<Schedule>> GetAllAsync();
    Task<Schedule?> GetByIdAsync(string id);
    Task<Schedule> CreateAsync(Schedule schedule);
    Task<Schedule?> UpdateAsync(string id, Schedule schedule);
    Task<bool> DeleteAsync(string id);
}
