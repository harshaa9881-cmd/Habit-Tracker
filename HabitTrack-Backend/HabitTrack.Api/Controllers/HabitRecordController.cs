using System.Security.Claims;

using HabitTrack.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitTrack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HabitRecordController : ControllerBase
{
    private readonly IHabitRecordService _service;

    public HabitRecordController(IHabitRecordService service)
    {
        _service = service;
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetDailySummary(
        [FromQuery] DateOnly? date)
    {
        DateOnly selectedDate = date ?? DateOnly.FromDateTime(DateTime.UtcNow);

        var summary = await _service.GetDailySummaryAsync(
            GetUserId(),
            selectedDate
        );

        return Ok(summary);
    }

    [HttpPost("{habitId:int}/complete")]
    public async Task<IActionResult> MarkCompleted(
        int habitId,
        [FromQuery] DateOnly date)
    {
        bool result = await _service.MarkCompletedAsync(
            habitId,
            GetUserId(),
            date
        );

        if (!result)
        {
            return NotFound(new
            {
                message = "Habit not found."
            });
        }

        return Ok(new
        {
            message = "Habit marked as completed."
        });
    }

    [HttpDelete("{habitId:int}/complete")]
    public async Task<IActionResult> MarkIncomplete(
        int habitId,
        [FromQuery] DateOnly date)
    {
        bool result = await _service.MarkIncompleteAsync(
            habitId,
            GetUserId(),
            date
        );

        if (!result)
        {
            return NotFound(new
            {
                message = "Habit not found."
            });
        }

        return Ok(new
        {
            message = "Habit marked as incomplete."
        });
    }
}