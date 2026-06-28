using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Application.Features.Reviews.DTOs;

namespace FoodLink.Application.Features.Reviews.Interfaces;

public interface IReviewQueries
{
    Task<PagedResponse<GroupedReviewResponse>> GetBusinessReviewsAsync(
        Guid businessId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<GroupedReviewResponse>> GetCharityReviewsAsync(
        Guid charityId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);

    Task<RatingSummaryResponse?> GetBusinessRatingSummaryAsync(
    Guid businessId,
    CancellationToken cancellationToken = default);

    Task<RatingSummaryResponse?> GetCharityRatingSummaryAsync(
        Guid charityId,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<GroupedReviewResponse>> GetMyBusinessReviewsAsync(
        Guid userId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<GroupedReviewResponse>> GetMyCharityReviewsAsync(
        Guid userId,
        PaginationRequest request,
        CancellationToken cancellationToken = default);
}