using HabitTrack.Api.DTOs.Auth;
using HabitTrack.Api.Services;

using Microsoft.AspNetCore.Mvc;

namespace HabitTrack.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        bool isRegistered = await _authService.RegisterAsync(request);

        if (!isRegistered)
        {
            return Conflict(new
            {
                message = "An account with this email already exists."
            });
        }

        return Ok(new
        {
            message = "Registration successful."
        });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await _authService.LoginAsync(request);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid email or password."
            });
        }

        return Ok(response);
    }
}