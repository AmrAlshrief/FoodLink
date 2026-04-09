using FoodLink.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
    public void Configure(EntityTypeBuilder<Review> builder)
    {
        builder.HasKey(rev => rev.Id);
        
        // Link to the specific reservation being reviewed
        builder.HasOne<Reservation>()
               .WithOne()
               .HasForeignKey<Review>(rev => rev.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(rev => rev.Rating).IsRequired();
        builder.Property(rev => rev.Comment).HasMaxLength(500);
    }
}