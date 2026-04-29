using Microsoft.EntityFrameworkCore;
using RunDMC.Models;

namespace RunDMC.Data;

public class FitnessDbContext(DbContextOptions<FitnessDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<ActivityType> ActivityTypes => Set<ActivityType>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.HasMany(u => u.Workouts)
                  .WithOne(w => w.User)
                  .HasForeignKey(w => w.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<ActivityType>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Name).IsRequired().HasMaxLength(50);
            entity.HasMany(a => a.Workouts)
                  .WithOne(w => w.ActivityType)
                  .HasForeignKey(w => w.ActivityTypeId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Workout>(entity =>
        {
            entity.HasKey(w => w.Id);
        });

        modelBuilder.Entity<ActivityType>().HasData(
            new ActivityType { Id = 1, Name = "Running" },
            new ActivityType { Id = 2, Name = "Cycling" },
            new ActivityType { Id = 3, Name = "Strength Training" },
            new ActivityType { Id = 4, Name = "Swimming" },
            new ActivityType { Id = 5, Name = "Walking" }
        );
    }
}
