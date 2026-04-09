using FoodLink.Domain.Entities;
using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.HasKey(r => r.Id);

        var navigation = builder.Metadata.FindNavigation(nameof(Reservation.Items));
        navigation?.SetPropertyAccessMode(PropertyAccessMode.Field);

        // Relationship to Charity (User)
        builder.HasOne<CharityProfile>()
               .WithMany()
               .HasForeignKey(r => r.CharityId)
               .OnDelete(DeleteBehavior.Restrict); // Don't delete charity if reservation exists

        builder.HasMany(r => r.Items)
               .WithOne()
               .HasForeignKey(ri => ri.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

public class ReservationItemConfiguration : IEntityTypeConfiguration<ReservationItem>
{
    public void Configure(EntityTypeBuilder<ReservationItem> builder)
    {
        builder.HasKey(ri => ri.Id);

        // Link to the specific food item
        builder.HasOne<DonationItem>()
               .WithMany()
               .HasForeignKey(ri => ri.DonationItemId)
               .OnDelete(DeleteBehavior.Restrict); 
    }
}