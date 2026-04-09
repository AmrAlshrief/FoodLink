using FoodLink.Domain.Common;
using FoodLink.Domain.Common.Exceptions;
namespace FoodLink.Domain.Entities.Profiles;

public abstract class Profile : AuditableEntity
{
    public Guid UserId { get; protected set; }

    public double AverageRating { get; protected set; }
    public int RatingCount { get; protected set; }

    protected Profile() : base() { }

    protected Profile(Guid id) : base(id) { }

    public void AddRating(int value)
    {
        if (value < 1 || value > 5)
            throw new DomainException("Rating must be between 1 and 5");

        AverageRating = ((AverageRating * RatingCount) + value) / (RatingCount + 1);
        RatingCount++;
    }
}