using FoodLink.Domain.Enums;
using FoodLink.Domain.Entities;

namespace FoodLink.Application.Features.Reviews.Interfaces;

public interface IReviewRepository
{
    Task AddAsync(
        Review review,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsAsync(
        Guid reservationId,
        Guid reviewerId,
        ReviewType type,
        CancellationToken cancellationToken = default);
}