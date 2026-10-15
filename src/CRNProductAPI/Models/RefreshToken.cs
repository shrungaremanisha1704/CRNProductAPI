using System.ComponentModel.DataAnnotations;

namespace CRNProductAPI.Models;

public class RefreshToken
{
    public int Id { get; set; }
    [Required]
    public string TokenHash { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public DateTime? RevokedAt { get; set; }
    public int AppUserId { get; set; }
    public AppUser? AppUser { get; set; }
}
