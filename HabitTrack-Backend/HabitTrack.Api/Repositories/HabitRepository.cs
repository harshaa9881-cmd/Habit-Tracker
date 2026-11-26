using HabitTrack.Api.Data;
using HabitTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTrack.Api.Repositories;

public class HabitRepository : IHabitRepository
{
    private readonly ApplicationDbContext _context;

    public HabitRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Habit>> GetAllAsync(int userId)
    {
        return await _context.Habits
            .Where(h => h.UserId == userId && h.IsActive)
            .OrderByDescending(h => h.CreatedAt)
            .ToListAsync();
    }

    public async Task<Habit?> GetByIdAsync(int habitId, int userId)
    {
        return await _context.Habits
            .FirstOrDefaultAsync(h =>
                h.HabitId == habitId &&
                h.UserId == userId &&
                h.IsActive);
    }

    public async Task AddAsync(Habit habit)
    {
        await _context.Habits.AddAsync(habit);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}