using GymManagementAPI.Models;
using GymManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementAPI.Controllers;

[ApiController]
[Route("api/members")]
[Authorize] // Require authentication for all endpoints
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var members = await _memberService.GetAllAsync();
            return Ok(members);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var member = await _memberService.GetByIdAsync(id);
            if (member == null)
            {
                return NotFound(new { error = "Member not found" });
            }
            return Ok(member);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "admin")] // Only admin can create members
    public async Task<IActionResult> Create([FromBody] Member member)
    {
        try
        {
            var createdMember = await _memberService.CreateAsync(member);
            return StatusCode(201, createdMember);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "admin")] // Only admin can update members
    public async Task<IActionResult> Update(string id, [FromBody] Member member)
    {
        try
        {
            member.Id = id;
            var updatedMember = await _memberService.UpdateAsync(id, member);
            if (updatedMember == null)
            {
                return NotFound(new { error = "Member not found" });
            }
            return Ok(updatedMember);
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "admin")] // Only admin can delete members
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            var deleted = await _memberService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound(new { error = "Member not found" });
            }
            return Ok(new { message = "Member deleted successfully" });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { error = ex.Message });
        }
    }
}
