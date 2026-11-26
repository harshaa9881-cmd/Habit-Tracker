using HabitTrack.Api.Models;

namespace HabitTrack.Api.Repositories;

public interface IHabitRecordRepository
{
    Task<List<Habit>> GetActiveHabitsAsync(int userId);

    Task<List<HabitRecord>> GetRecordsByDateAsync(
        int userId,
        DateOnly date);

    Task<Habit?> GetActiveHabitAsync(
        int habitId,
        int userId);

    Task AddAsync(HabitRecord record);

    Task DeleteAsync(HabitRecord record);

    Task SaveChangesAsync();
}