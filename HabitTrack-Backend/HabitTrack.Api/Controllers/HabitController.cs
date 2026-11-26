using System.Security.Claims;

using HabitTrack.Api.DTOs.Habit;
using HabitTrack.Api.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HabitTrack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class HabitController : ControllerBase
{
    private readonly IHabitService _habitService;

    public HabitController(IHabitService habitService)
    {
        _habitService = habitService;
    }

    private int GetUserId()
    {
        return int.Parse(
            User.FindFirstValue(ClaimTypes.NameIdentifier)!
        );
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        int userId = GetUserId();

        var habits = await _habitService.GetAllAsync(userId);

        return Ok(habits);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        int userId = GetUserId();

        var habit = await _habitService.GetByIdAsync(id, userId);

        if (habit == null)
        {
            return NotFound(new
            {
                message = "Habit not found."
            });
        }

        return Ok(habit);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateHabitRequest request)
    {
        int userId = GetUserId();

        var habit = await _habitService.CreateAsync(userId, request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = habit.HabitId },
            habit
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(
        int id,
        UpdateHabitRequest request)
    {
        int userId = GetUserId();

        var habit = await _habitService.UpdateAsync(
            id,
            userId,
            request
        );

        if (habit == null)
        {
            return NotFound(new
            {
                message = "Habit not found."
            });
        }

        return Ok(habit);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        int userId = GetUserId();

        bool isDeleted = await _habitService.DeleteAsync(id, userId);

        if (!isDeleted)
        {
            return NotFound(new
            {
                message = "Habit not found."
            });
        }

        return NoContent();
    }
}