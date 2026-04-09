using FoodLink.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Configurations;

public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
{
    public void Configure(EntityTypeBuilder<Notification> builder)
    {
        builder.HasKey(n => n.Id);
        builder.Property(n => n.Title).IsRequired().HasMaxLength(100);
        builder.Property(n => n.Message).IsRequired().HasMaxLength(500);
        
        // Index for performance when fetching unread notifications for a user
        builder.HasIndex(n => new { n.UserId, n.IsRead });
    }
}