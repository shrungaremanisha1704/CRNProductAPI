using CRNProductAPI.Models;
using CRNProductAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CRNProductAPI.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class AuthController(IAuthService auth) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await auth.RegisterAsync(request);
        if (result.Conflict) return Conflict(new { message = result.Message });
        return result.Success ? Ok(new { message = result.Message }) : BadRequest(new { message = result.Message });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var response = await auth.LoginAsync(request);
        return response is null ? Unauthorized(new { message = "Invalid username or password." }) : Ok(response);
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var response = await auth.RefreshAsync(request.RefreshToken);
        return response is null ? Unauthorized(new { message = "Invalid or expired refresh token." }) : Ok(response);
    }
}
