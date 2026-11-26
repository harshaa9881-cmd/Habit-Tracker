
using HabitTrack.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace HabitTrack.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<ApplicationUser> Users { get; set; }

    public DbSet<Habit> Habits { get; set; }

    public DbSet<HabitRecord> HabitRecords { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ApplicationUser primary key
        modelBuilder.Entity<ApplicationUser>()
            .HasKey(u => u.UserId);

        // Unique email
        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Habit relationship
        modelBuilder.Entity<Habit>()
            .HasOne<ApplicationUser>()
            .WithMany()
            .HasForeignKey(h => h.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // HabitRecord relationship
        modelBuilder.Entity<HabitRecord>()
            .HasOne<Habit>()
            .WithMany()
            .HasForeignKey(r => r.HabitId)
            .OnDelete(DeleteBehavior.Cascade);

        // Prevent duplicate completion records
        // for the same habit and date
        modelBuilder.Entity<HabitRecord>()
            .HasIndex(r => new
            {
                r.HabitId,
                r.CompletionDate
            })
            .IsUnique();
    }
}
