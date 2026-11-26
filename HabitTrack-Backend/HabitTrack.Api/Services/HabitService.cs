using HabitTrack.Api.DTOs.Habit;
using HabitTrack.Api.Models;
using HabitTrack.Api.Repositories;

namespace HabitTrack.Api.Services;

public class HabitService : IHabitService
{
    private readonly IHabitRepository _repository;

    public HabitService(IHabitRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<HabitResponse>> GetAllAsync(int userId)
    {
        var habits = await _repository.GetAllAsync(userId);

        return habits.Select(h => new HabitResponse
        {
            HabitId = h.HabitId,
            Name = h.Name,
            Description = h.Description,
            CreatedAt = h.CreatedAt
        }).ToList();
    }

    public async Task<HabitResponse?> GetByIdAsync(
        int habitId,
        int userId)
    {
        var habit = await _repository.GetByIdAsync(habitId, userId);

        if (habit == null)
        {
            return null;
        }

        return MapToResponse(habit);
    }

    public async Task<HabitResponse> CreateAsync(
        int userId,
        CreateHabitRequest request)
    {
        var habit = new Habit
        {
            UserId = userId,
            Name = request.Name.Trim(),
            Description = request.Description?.Trim()
        };

        await _repository.AddAsync(habit);

        await _repository.SaveChangesAsync();

        return MapToResponse(habit);
    }

    public async Task<HabitResponse?> UpdateAsync(
        int habitId,
        int userId,
        UpdateHabitRequest request)
    {
        var habit = await _repository.GetByIdAsync(habitId, userId);

        if (habit == null)
        {
            return null;
        }

        habit.Name = request.Name.Trim();
        habit.Description = request.Description?.Trim();

        await _repository.SaveChangesAsync();

        return MapToResponse(habit);
    }

    public async Task<bool> DeleteAsync(int habitId, int userId)
    {
        var habit = await _repository.GetByIdAsync(habitId, userId);

        if (habit == null)
        {
            return false;
        }

        habit.IsActive = false;

        await _repository.SaveChangesAsync();

        return true;
    }

    private static HabitResponse MapToResponse(Habit habit)
    {
        return new HabitResponse
        {
            HabitId = habit.HabitId,
            Name = habit.Name,
            Description = habit.Description,
            CreatedAt = habit.CreatedAt
        };
    }
}