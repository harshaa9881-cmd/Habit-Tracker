using HabitTrack.Api.Models;

namespace HabitTrack.Api.Repositories;

public interface IHabitRepository
{
    Task<List<Habit>> GetAllAsync(int userId);

    Task<Habit?> GetByIdAsync(int habitId, int userId);

    Task AddAsync(Habit habit);

    Task SaveChangesAsync();
}