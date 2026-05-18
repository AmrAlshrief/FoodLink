using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class CharityProfileConfiguration : IEntityTypeConfiguration<CharityProfile>
{
    public void Configure(EntityTypeBuilder<CharityProfile> builder)
    {
        builder.HasKey(cp => cp.Id);
        
        
        builder.Property(cp => cp.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(cp => cp.LicenseNumber)
               .IsRequired()
               .HasMaxLength(50);
               
        builder.Property(cp => cp.Address)
               .IsRequired()
               .HasMaxLength(200);
    }
}
