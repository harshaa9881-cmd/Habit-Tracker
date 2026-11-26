using HabitTrack.Api.Data;
using HabitTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTrack.Api.Repositories;

public class HabitRecordRepository : IHabitRecordRepository
{
    private readonly ApplicationDbContext _context;

    public HabitRecordRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Habit>> GetActiveHabitsAsync(int userId)
    {
        return await _context.Habits
            .Where(h => h.UserId == userId && h.IsActive)
            .OrderBy(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<HabitRecord>> GetRecordsByDateAsync(
        int userId,
        DateOnly date)
    {
        return await _context.HabitRecords
            .Where(r =>
                r.UserId == userId &&
                r.CompletionDate == date)
            .ToListAsync();
    }

    public async Task<Habit?> GetActiveHabitAsync(
        int habitId,
        int userId)
    {
        return await _context.Habits
            .FirstOrDefaultAsync(h =>
                h.HabitId == habitId &&
                h.UserId == userId &&
                h.IsActive);
    }

    public async Task AddAsync(HabitRecord record)
    {
        await _context.HabitRecords.AddAsync(record);
    }

    public async Task DeleteAsync(HabitRecord record)
    {
        _context.HabitRecords.Remove(record);
        await Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}