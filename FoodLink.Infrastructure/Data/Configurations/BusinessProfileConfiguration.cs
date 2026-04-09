using FoodLink.Domain.Entities;
using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class BusinessProfileConfiguration : IEntityTypeConfiguration<BusinessProfile>
{
    public void Configure(EntityTypeBuilder<BusinessProfile> builder)
    {
        builder.HasKey(bp => bp.Id);
        
        // Link to User
        builder.HasOne<User>()
               .WithOne()
               .HasForeignKey<BusinessProfile>(bp => bp.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.Property(bp => bp.BusinessName).IsRequired().HasMaxLength(100);
    }
}