using GymManagementAPI.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using GymManagementAPI.Models;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IMemberService _memberService;
    private readonly ICoachService _coachService;
    private readonly IPaymentService _paymentService;
    private readonly IMongoDatabase _database;

    public DashboardController(
        IMemberService memberService, 
        ICoachService coachService, 
        IPaymentService paymentService,
        IMongoDatabase database)
    {
        _memberService = memberService;
        _coachService = coachService;
        _paymentService = paymentService;
        _database = database;
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        try
        {
            var membersCollection = _database.GetCollection<Member>("members");
            var coachesCollection = _database.GetCollection<Coach>("coaches");

            var totalMembers = await membersCollection.CountDocumentsAsync(_ => true);
            var activeMembers = await membersCollection.CountDocumentsAsync(m => m.Status == "active");
            var totalCoaches = await coachesCollection.CountDocumentsAsync(_ => true);
            var totalRevenue = await _paymentService.GetTotalRevenueAsync();

            return Ok(new
            {
                totalMembers,
                activeMembers,
                totalCoaches,
                totalRevenue
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
