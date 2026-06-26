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
        builder.HasOne(rev => rev.Reservation)
               .WithMany()
               .HasForeignKey(rev => rev.ReservationId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => new
        {
            x.ReservationId,
            x.ReviewerId,
            x.Type
        }).IsUnique();

        builder.Property(rev => rev.Rating).IsRequired();
        builder.Property(rev => rev.Comment).HasMaxLength(500);
    }
}