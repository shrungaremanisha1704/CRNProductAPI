using CRNProductAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CRNProductAPI.Data;

public class ApplicationDbContext(
    DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<AppUser> AppUsers => Set<AppUser>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Product configuration
        modelBuilder.Entity<Product>()
            .HasIndex(p => p.ProductName);

        // Item and Product relationship
        modelBuilder.Entity<Item>()
            .HasOne(i => i.Product)
            .WithMany(p => p.Items)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // AppUser configuration
        modelBuilder.Entity<AppUser>()
            .HasIndex(u => u.Username)
            .IsUnique();

        // RefreshToken: map TokenHash property to the existing SQL column "Token"
        modelBuilder.Entity<RefreshToken>()
            .Property(r => r.TokenHash)
            .HasColumnName("Token");

        // RefreshToken unique index
        modelBuilder.Entity<RefreshToken>()
            .HasIndex(r => r.TokenHash)
            .IsUnique();

        // RefreshToken and AppUser relationship
        modelBuilder.Entity<RefreshToken>()
            .HasOne(r => r.AppUser)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.AppUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}