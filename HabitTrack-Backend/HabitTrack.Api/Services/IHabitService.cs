using HabitTrack.Api.DTOs.Habit;

namespace HabitTrack.Api.Services;

public interface IHabitService
{
    Task<List<HabitResponse>> GetAllAsync(int userId);

    Task<HabitResponse?> GetByIdAsync(int habitId, int userId);

    Task<HabitResponse> CreateAsync(
        int userId,
        CreateHabitRequest request);

    Task<HabitResponse?> UpdateAsync(
        int habitId,
        int userId,
        UpdateHabitRequest request);

    Task<bool> DeleteAsync(int habitId, int userId);
}