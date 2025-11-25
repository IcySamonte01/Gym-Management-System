using GymManagementAPI.Models;
using MongoDB.Driver;

namespace GymManagementAPI.Services;

public class ScheduleService : IScheduleService
{
    private readonly IMongoCollection<Schedule> _schedules;
    private readonly IMongoCollection<Coach> _coaches;

    public ScheduleService(IMongoDatabase database)
    {
        _schedules = database.GetCollection<Schedule>("schedules");
        _coaches = database.GetCollection<Coach>("coaches");
    }

    public async Task<List<Schedule>> GetAllAsync()
    {
        var schedules = await _schedules.Find(_ => true).ToListAsync();
        
        // Populate coach information
        foreach (var schedule in schedules)
        {
            schedule.Coach = await _coaches.Find(c => c.Id == schedule.CoachId).FirstOrDefaultAsync();
        }

        return schedules;
    }

    public async Task<Schedule?> GetByIdAsync(string id)
    {
        return await _schedules.Find(s => s.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Schedule> CreateAsync(Schedule schedule)
    {
        schedule.CreatedAt = DateTime.UtcNow;
        schedule.UpdatedAt = DateTime.UtcNow;
        await _schedules.InsertOneAsync(schedule);
        return schedule;
    }

    public async Task<Schedule?> UpdateAsync(string id, Schedule schedule)
    {
        schedule.UpdatedAt = DateTime.UtcNow;
        schedule.Id = id;
        
        await _schedules.ReplaceOneAsync(s => s.Id == id, schedule);
        return schedule;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await _schedules.DeleteOneAsync(s => s.Id == id);
        return result.DeletedCount > 0;
    }
}
