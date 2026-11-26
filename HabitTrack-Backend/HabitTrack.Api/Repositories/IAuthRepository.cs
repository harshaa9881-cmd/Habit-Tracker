using HabitTrack.Api.Models;

namespace HabitTrack.Api.Repositories;

public interface IAuthRepository
{
    Task<ApplicationUser?> GetByEmailAsync(string email);

    Task AddAsync(ApplicationUser user);

    Task SaveChangesAsync();
}