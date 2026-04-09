using FoodLink.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class DonationConfiguration : IEntityTypeConfiguration<Donation>
{
    public void Configure(EntityTypeBuilder<Donation> builder)
{
    builder.ToTable("Donations");

    builder.HasKey(d => d.Id);

    builder.Property(d => d.Title)
           .IsRequired()
           .HasMaxLength(100);

    builder.Property(d => d.Description)
           .HasMaxLength(500);

    builder.Property(d => d.ImageUrl)
           .HasMaxLength(500);

    builder.Property(d => d.Status)
           .HasConversion<string>()
           .HasMaxLength(20);

    builder.Property(d => d.ExpiryDate)
           .IsRequired();

    builder.Property(d => d.BusinessProfileId)
           .IsRequired();

    builder.HasIndex(d => d.Status);
    builder.HasIndex(d => d.ExpiryDate);
    builder.HasIndex(d => d.BusinessProfileId);

    builder.HasMany(d => d.Items)
           .WithOne()
           .HasForeignKey(i => i.DonationId)
           .OnDelete(DeleteBehavior.Cascade);

    builder.Navigation(d => d.Items)
           .UsePropertyAccessMode(PropertyAccessMode.Field);
}
}