using Microsoft.EntityFrameworkCore;
using UserApp.Core.Domain.Entities;

namespace UserApp.Infrastructure.Data.Context;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
  public DbSet<User> Users { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.HasOne(e => e.Profile)
                  .WithOne(e => e.User)
                  .HasForeignKey<UserProfile>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<UserProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
        });
    }
}