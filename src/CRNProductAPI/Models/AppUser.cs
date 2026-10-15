using System.ComponentModel.DataAnnotations;

namespace CRNProductAPI.Models;

public class AppUser
{
    public int Id { get; set; }
    [Required, StringLength(100)]
    public string Username { get; set; } = string.Empty;
    [Required]
    public string PasswordHash { get; set; } = string.Empty;
    [Required, StringLength(30)]
    public string Role { get; set; } = "User";
    public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}
