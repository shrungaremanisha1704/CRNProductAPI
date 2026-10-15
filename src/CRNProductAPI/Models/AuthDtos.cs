using System.ComponentModel.DataAnnotations;

namespace CRNProductAPI.Models;

public class RegisterRequest
{
    [Required, StringLength(100, MinimumLength = 3)]
    public string Username { get; set; } = string.Empty;
    [Required, StringLength(100, MinimumLength = 8)]
    public string Password { get; set; } = string.Empty;
}
public class LoginRequest
{
    [Required] public string Username { get; set; } = string.Empty;
    [Required] public string Password { get; set; } = string.Empty;
}
public class RefreshRequest
{
    [Required] public string RefreshToken { get; set; } = string.Empty;
}
public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt { get; set; }
}
