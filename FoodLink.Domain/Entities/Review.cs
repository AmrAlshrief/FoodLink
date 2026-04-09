using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class Review : AuditableEntity
{
    public Guid ReservationId { get; private set; }
    public Guid ReviewerId { get; private set; } // The User ID of the person writing it
    public Guid TargetId { get; private set; }   // The User ID of the person receiving it
    
    public int Rating { get; private set; }      // Constraint: 1 to 5
    public string? Comment { get; private set; }

    private Review() { }

    public Review(Guid reservationId, Guid reviewerId, Guid targetId, int rating, string? comment) 
        : base(Guid.NewGuid())
    {
        // Business Rule: Standardize the rating scale
        if (rating < 1 || rating > 5) 
            throw new DomainException("Rating must be between 1 and 5.");

        // Business Rule: A user shouldn't be able to rate themselves
        if (reviewerId == targetId)
            throw new DomainException("You cannot rate yourself.");

        ReservationId = reservationId;
        ReviewerId = reviewerId;
        TargetId = targetId;
        Rating = rating;
        Comment = comment;
    }
}