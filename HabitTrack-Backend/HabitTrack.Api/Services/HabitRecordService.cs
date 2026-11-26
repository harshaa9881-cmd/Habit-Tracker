using HabitTrack.Api.DTOs.HabitRecord;
using HabitTrack.Api.Models;
using HabitTrack.Api.Repositories;

namespace HabitTrack.Api.Services;

public class HabitRecordService : IHabitRecordService
{
    private readonly IHabitRecordRepository _repository;

    public HabitRecordService(IHabitRecordRepository repository)
    {
        _repository = repository;
    }

    public async Task<DailySummaryResponse> GetDailySummaryAsync(
        int userId,
        DateOnly date)
    {
        var habits = await _repository.GetActiveHabitsAsync(userId);

        var records = await _repository.GetRecordsByDateAsync(
            userId,
            date
        );

        var completedHabitIds = records
            .Select(r => r.HabitId)
            .ToHashSet();

        var dailyHabits = habits.Select(h => new DailyHabitResponse
        {
            HabitId = h.HabitId,
            Name = h.Name,
            Description = h.Description,
            IsCompleted = completedHabitIds.Contains(h.HabitId)
        }).ToList();

        int total = dailyHabits.Count;

        int completed = dailyHabits.Count(h => h.IsCompleted);

        int remaining = total - completed;

        int percentage = total == 0
            ? 0
            : (int)Math.Round((double)completed / total * 100);

        return new DailySummaryResponse
        {
            Date = date,
            TotalHabits = total,
            CompletedHabits = completed,
            RemainingHabits = remaining,
            CompletionPercentage = percentage,
            Habits = dailyHabits
        };
    }

    public async Task<bool> MarkCompletedAsync(
        int habitId,
        int userId,
        DateOnly date)
    {
        var habit = await _repository.GetActiveHabitAsync(
            habitId,
            userId
        );

        if (habit == null)
        {
            return false;
        }

        var existingRecords = await _repository.GetRecordsByDateAsync(
            userId,
            date
        );

        if (existingRecords.Any(r => r.HabitId == habitId))
        {
            return true;
        }

        var record = new HabitRecord
        {
            HabitId = habitId,
            UserId = userId,
            CompletionDate = date,
            CompletedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(record);

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<bool> MarkIncompleteAsync(
        int habitId,
        int userId,
        DateOnly date)
    {
        var habit = await _repository.GetActiveHabitAsync(
            habitId,
            userId
        );

        if (habit == null)
        {
            return false;
        }

        var records = await _repository.GetRecordsByDateAsync(
            userId,
            date
        );

        var record = records.FirstOrDefault(r => r.HabitId == habitId);

        if (record == null)
        {
            return true;
        }

        await _repository.DeleteAsync(record);

        await _repository.SaveChangesAsync();

        return true;
    }
}