using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Application.Features.Reviews.DTOs;

namespace FoodLink.Application.Features.Reviews.Interfaces;

public interface IReviewQueries
{
    Task<PagedResponse<ReviewResponse>> GetBusinessReviewsAsync(
        Guid businessId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<ReviewResponse>> GetCharityReviewsAsync(
        Guid charityId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);

    Task<RatingSummaryResponse?> GetBusinessRatingSummaryAsync(
    Guid businessId,
    CancellationToken cancellationToken = default);

    Task<RatingSummaryResponse?> GetCharityRatingSummaryAsync(
        Guid charityId,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<ReviewResponse>> GetMyBusinessReviewsAsync(
        Guid userId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<ReviewResponse>> GetMyCharityReviewsAsync(
        Guid userId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);
}