using HabitTrack.Api.DTOs.HabitRecord;

namespace HabitTrack.Api.Services;

public interface IHabitRecordService
{
    Task<DailySummaryResponse> GetDailySummaryAsync(
        int userId,
        DateOnly date);

    Task<bool> MarkCompletedAsync(
        int habitId,
        int userId,
        DateOnly date);

    Task<bool> MarkIncompleteAsync(
        int habitId,
        int userId,
        DateOnly date);
}