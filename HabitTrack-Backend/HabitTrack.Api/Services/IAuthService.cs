using HabitTrack.Api.DTOs.Auth;

namespace HabitTrack.Api.Services;

public interface IAuthService
{
    Task<bool> RegisterAsync(RegisterRequest request);

    Task<AuthResponse?> LoginAsync(LoginRequest request);
}