using FoodLink.Domain.Entities;
using FoodLink.Domain.Entities.Profiles;
using FoodLink.Domain.Common;
using FoodLink.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data;

public class AppDbContext : DbContext
{
    private readonly AuditableEntityInterceptor _auditableInterceptor;

    public AppDbContext(
        DbContextOptions<AppDbContext> options, 
        AuditableEntityInterceptor auditableInterceptor) : base(options)
    {
        _auditableInterceptor = auditableInterceptor;
    }

    // Aggregates & Independent Entities
    public DbSet<User> Users => Set<User>();
    public DbSet<BusinessProfile> BusinessProfiles => Set<BusinessProfile>();
    public DbSet<CharityProfile> CharityProfiles => Set<CharityProfile>();
    public DbSet<Donation> Donations => Set<Donation>();
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<Review> Reviews => Set<Review>();
    public DbSet<Notification> Notifications => Set<Notification>();

    // Note: We do NOT create DbSets for DonationItem or ReservationItem 
    // because they are managed via their Aggregate Roots (Donation/Reservation).

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // This hooks up your automatic auditing logic
        optionsBuilder.AddInterceptors(_auditableInterceptor);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // This line is powerful: it finds all your "Configuration" classes 
        // in this project and applies them automatically.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}