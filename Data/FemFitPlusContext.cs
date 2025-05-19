using System;
using FemFitPlus.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FemFitPlus.Data;

public class FemFitPlusContext(DbContextOptions<FemFitPlusContext> options) : IdentityDbContext<FemFitUser>(options)
{
    public DbSet<WorkOut> Workouts { get; set; }
    public DbSet<Subscription> Subscriptions { get; set; }
    public DbSet<MealPlan> Mealplans { get; set; }
    public DbSet<Cycle> Cycles { get; set; }
    public DbSet<Profile> Profiles { get; set; }
    public DbSet<Consultation> Consultations { get; set; }
    public DbSet<WorkoutHistory> WorkoutHistories { get; set; }
    public DbSet<BodyMetric> BodyMetrics { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // This is needed for Identity tables

        // Optional: Customize the Identity tables
        builder.Entity<FemFitUser>().ToTable("AspNetUsers");
        builder.Entity<IdentityRole>().ToTable("AspNetRoles");
        builder.Entity<IdentityUserRole<string>>().ToTable("AspNetUserRoles");
        builder.Entity<IdentityUserClaim<string>>().ToTable("AspNetUserClaims");
        builder.Entity<IdentityUserLogin<string>>().ToTable("AspNetUserLogins");
        builder.Entity<IdentityRoleClaim<string>>().ToTable("AspNetRoleClaims");
        builder.Entity<IdentityUserToken<string>>().ToTable("AspNetUserTokens");

        // Configure one-to-one relationship between FemFitUser and Profile
        builder.Entity<FemFitUser>()
            .HasOne(u => u.Profile)
            .WithOne(p => p.Femfituser)
            .HasForeignKey<Profile>(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);


        // Configure one-to-one relationship between FemFitUser and Subscription
        builder.Entity<FemFitUser>()
            .HasMany(u => u.Subscriptions)
            .WithOne(s => s.Femfituser)
            .HasForeignKey(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between FemFitUser and Consultation
        builder.Entity<FemFitUser>()
            .HasMany(u => u.Consultations)
            .WithOne(c => c.Femfituser)
            .HasForeignKey(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between FemFitUser and WorkoutHistory
        builder.Entity<FemFitUser>()
            .HasMany(u => u.WorkoutHistory)
            .WithOne(wh => wh.Femfituser)
            .HasForeignKey(wh => wh.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between FemFitUser and Cycle
        builder.Entity<FemFitUser>()
            .HasMany(u => u.Cycles)
            .WithOne(c => c.Femfituser)
            .HasForeignKey(c => c.Id)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure one-to-many relationship between FemFitUser and BodyMetric
        builder.Entity<FemFitUser>()
            .HasMany(u => u.BodyMetrics)
            .WithOne(bm => bm.Femfituser)
            .HasForeignKey(bm => bm.Id)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Cycle>()
            .HasOne(u => u.Femfituser)
            .WithMany(c => c.Cycles)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // builder.Entity<Profile>()
        //     .HasOne(u => u.Femfituser)
        //     .WithOne(c => c.Profile)
        //     .HasForeignKey<Profile>(c => c.UserId)
        //     .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Subscription>()
            .HasOne(u => u.Femfituser)
            .WithMany(c => c.Subscriptions)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<WorkoutHistory>()
            .HasOne(u => u.Femfituser)
            .WithMany(c => c.WorkoutHistory)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Consultation>()
            .HasOne(u => u.Femfituser)
            .WithMany(c => c.Consultations)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<BodyMetric>()
            .HasOne(u => u.Femfituser)
            .WithMany(c => c.BodyMetrics)
            .HasForeignKey(c => c.FemFitUserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}