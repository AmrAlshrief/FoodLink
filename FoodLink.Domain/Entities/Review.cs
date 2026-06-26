using FoodLink.Domain.Common;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities;

public class Review : AuditableEntity
{
    public Guid ReservationId { get; private set; }

    public Guid ReviewerId { get; private set; }

    public Guid TargetId { get; private set; }

    public ReviewType Type { get; private set; }

    public int Rating { get; private set; }

    public string? Comment { get; private set; }

    public Reservation Reservation { get; private set; } = null!;

    private Review() { }

    private Review(
        Guid reservationId,
        Guid reviewerId,
        Guid targetId,
        ReviewType type,
        int rating,
        string? comment)
        : base(Guid.NewGuid())
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("Rating must be between 1 and 5.");

        if (reviewerId == targetId)
            throw new DomainException("You cannot rate yourself.");

        ReservationId = reservationId;
        ReviewerId = reviewerId;
        TargetId = targetId;
        Type = type;
        Rating = rating;
        Comment = comment;
    }

    public static Review Create(
        Guid reservationId,
        Guid reviewerId,
        Guid targetId,
        ReviewType type,
        int rating,
        string? comment)
    {
        return new Review(
            reservationId,
            reviewerId,
            targetId,
            type,
            rating,
            comment);
    }

    public void Update(int rating, string? comment)
    {
        if (rating < 1 || rating > 5)
            throw new DomainException("Rating must be between 1 and 5.");

        Rating = rating;
        Comment = comment;
    }
}