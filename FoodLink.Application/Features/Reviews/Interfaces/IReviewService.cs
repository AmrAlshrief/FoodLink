using FoodLink.Application.Features.Reviews.DTOs;

namespace FoodLink.Application.Features.Reviews.Interfaces;

public interface IReviewService
{
    Task CreateReviewAsync(
        Guid currentUserId,
        CreateReviewRequest request,
        CancellationToken cancellationToken = default);
}