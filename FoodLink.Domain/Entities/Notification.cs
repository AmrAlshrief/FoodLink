using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class Notification : AuditableEntity
{
    public Guid UserId { get; private set; }
    public string Title { get; private set; }
    public string Message { get; private set; }
    public string TargetType { get; private set; } // "Donation", "Reservation"
    public Guid? RelatedEntityId { get; private set; } // The ID of the Donation/Reservation
    public bool IsRead { get; private set; }

    private Notification() { }

    public Notification(Guid userId, string title, string message, string targetType, Guid? relatedId = null) 
        : base(Guid.NewGuid())
    {
        UserId = userId;
        Title = title;
        Message = message;
        TargetType = targetType;
        RelatedEntityId = relatedId;
        IsRead = false;
    }

    public void MarkAsRead()
    {
        IsRead = true;
    }
}