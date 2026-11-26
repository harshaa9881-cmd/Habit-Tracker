using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using HabitTrack.Api.DTOs.Auth;
using HabitTrack.Api.Models;
using HabitTrack.Api.Repositories;

using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace HabitTrack.Api.Services;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<ApplicationUser> _passwordHasher;

    public AuthService(
        IAuthRepository repository,
        IConfiguration configuration)
    {
        _repository = repository;
        _configuration = configuration;
        _passwordHasher = new PasswordHasher<ApplicationUser>();
    }

    public async Task<bool> RegisterAsync(RegisterRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        var existingUser = await _repository.GetByEmailAsync(email);

        if (existingUser != null)
        {
            return false;
        }

        var user = new ApplicationUser
        {
            FullName = request.FullName.Trim(),
            Email = email
        };

        user.PasswordHash = _passwordHasher.HashPassword(
            user,
            request.Password
        );

        await _repository.AddAsync(user);

        await _repository.SaveChangesAsync();

        return true;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        string email = request.Email.Trim().ToLowerInvariant();

        var user = await _repository.GetByEmailAsync(email);

        if (user == null)
        {
            return null;
        }

        var passwordResult = _passwordHasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password
        );

        if (passwordResult == PasswordVerificationResult.Failed)
        {
            return null;
        }

        string token = GenerateToken(user);

        return new AuthResponse
        {
            Token = token,
            FullName = user.FullName,
            Email = user.Email
        };
    }

    private string GenerateToken(ApplicationUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");

        string secretKey = jwtSettings["SecretKey"]!;
        string issuer = jwtSettings["Issuer"]!;
        string audience = jwtSettings["Audience"]!;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey)
        );

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256
        );

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            new Claim(ClaimTypes.Name, user.FullName),
            new Claim(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}