using FoodLink.Application.Features.Reviews.DTOs;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Application.Common.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/reviews")]
[Authorize]
public class ReviewController(
    IReviewService reviewService,
    IUserContext userContext,
    IReviewQueries reviewQueries)
    : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateReviewRequest request,
        CancellationToken cancellationToken)
    {
        await reviewService.CreateReviewAsync(
            userContext.UserId!.Value,
            request,
            cancellationToken);

        return Ok(new
        {
            message = "Review submitted successfully."
        });
    }

    [HttpGet("business/{businessId}/summary")]
    public async Task<IActionResult> GetBusinessRatingSummary(
        Guid businessId,
        CancellationToken cancellationToken)
    {
        var result = await reviewQueries
            .GetBusinessRatingSummaryAsync(
                businessId,
                cancellationToken);

        return Ok(result);
    }

    [HttpGet("charity/{charityId}/summary")]
    public async Task<IActionResult> GetCharityRatingSummary(
        Guid charityId,
        CancellationToken cancellationToken)
    {
        var result = await reviewQueries
            .GetCharityRatingSummaryAsync(
            charityId,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("business/{businessId}")]
    public async Task<IActionResult> GetBusinessReviews(
        Guid businessId,
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await reviewQueries.GetBusinessReviewsAsync(
            businessId,
            request,
            cancellationToken);

        return Ok(result);
    }

    [HttpGet("charity/{charityId}")]
    public async Task<IActionResult> GetCharityReviews(
        Guid charityId,
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var result = await reviewQueries.GetCharityReviewsAsync(
            charityId,
            request,
            cancellationToken);

        return Ok(result);
    }

    
}