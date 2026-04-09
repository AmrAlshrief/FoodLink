using FoodLink.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class DonationItemConfiguration : IEntityTypeConfiguration<DonationItem>
{
    public void Configure(EntityTypeBuilder<DonationItem> builder)
    {
        builder.HasKey(di => di.Id);
        builder.Property(di => di.Name).IsRequired().HasMaxLength(100);
        //builder.Property(di => di.Unit).IsRequired().HasMaxLength(20);
        builder.Property(di => di.ImageUrl).HasMaxLength(500);
        
        // Ensure quantities aren't negative at DB level
        builder.ToTable(t => t.HasCheckConstraint("CK_DonationItem_Quantity", "\"TotalQuantity\" >= 0"));
    }
}