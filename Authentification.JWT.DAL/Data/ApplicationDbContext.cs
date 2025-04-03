using Authentification.JWT.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Authentification.JWT.DAL.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<JwtRefreshToken> JwtRefreshTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(100);

            entity.HasIndex(e=>e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        modelBuilder.Entity<JwtRefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}