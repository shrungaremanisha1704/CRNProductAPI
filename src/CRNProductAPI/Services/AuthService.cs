using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using CRNProductAPI.Data;
using CRNProductAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace CRNProductAPI.Services;

public interface ITokenService
{
    Task<AuthResponse> IssueAsync(AppUser user);
    string Hash(string token);
}
public interface IAuthService
{
    Task<(bool Success, string Message, AuthResponse? Response, bool Conflict)> RegisterAsync(RegisterRequest request);
    Task<AuthResponse?> LoginAsync(LoginRequest request);
    Task<AuthResponse?> RefreshAsync(string refreshToken);
}
public class TokenService(ApplicationDbContext db, IConfiguration configuration) : ITokenService
{
    public string Hash(string token) => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));

    public async Task<AuthResponse> IssueAsync(AppUser user)
    {
        var keyText = configuration["Jwt:Key"] ?? throw new InvalidOperationException("JWT key is missing.");
        var now = DateTime.UtcNow;
        var expiry = now.AddMinutes(configuration.GetValue("Jwt:AccessTokenMinutes", 15));
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyText));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var token = new JwtSecurityToken(configuration["Jwt:Issuer"], configuration["Jwt:Audience"],
            claims, now, expiry, creds);
        var rawRefresh = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
        db.RefreshTokens.Add(new RefreshToken
        {
            TokenHash = Hash(rawRefresh), AppUserId = user.Id,
            ExpiresAt = now.AddDays(configuration.GetValue("Jwt:RefreshTokenDays", 7))
        });
        await db.SaveChangesAsync();
        return new AuthResponse { AccessToken = new JwtSecurityTokenHandler().WriteToken(token), RefreshToken = rawRefresh, AccessTokenExpiresAt = expiry };
    }
}
public class AuthService(ApplicationDbContext db, ITokenService tokens) : IAuthService
{
    private readonly PasswordHasher<AppUser> _hasher = new();

    public async Task<(bool Success, string Message, AuthResponse? Response, bool Conflict)> RegisterAsync(RegisterRequest request)
    {
        var username = request.Username.Trim();
        if (username.Length < 3) return (false, "Username must contain at least 3 characters.", null, false);
        if (await db.AppUsers.AnyAsync(u => u.Username == username))
            return (false, "Username already exists.", null, true);
        var user = new AppUser { Username = username, Role = "User" };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        db.AppUsers.Add(user);
        await db.SaveChangesAsync();
        return (true, "Registration successful.", null, false);
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request)
    {
        var user = await db.AppUsers.FirstOrDefaultAsync(u => u.Username == request.Username.Trim());
        if (user is null || _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return null;
        return await tokens.IssueAsync(user);
    }

    public async Task<AuthResponse?> RefreshAsync(string rawToken)
    {
        var hash = tokens.Hash(rawToken);
        var stored = await db.RefreshTokens.Include(t => t.AppUser)
            .FirstOrDefaultAsync(t => t.TokenHash == hash);
        if (stored is null || stored.RevokedAt != null || stored.ExpiresAt <= DateTime.UtcNow || stored.AppUser is null)
            return null;
        stored.RevokedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();
        return await tokens.IssueAsync(stored.AppUser);
    }
}
