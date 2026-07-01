using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Businesses.Interfaces;
using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Application.Features.Businesses.DTOs;
using FoodLink.Application.Common.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BusinessController(IBusinessService businessService, IReviewQueries reviewQueries, IUserContext userContext) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<BusinessPublicProfileResponse>> GetPublicProfile(Guid id, CancellationToken ct)
    {
        var profile = await businessService.GetPublicProfileAsync(id, ct);
        return Ok(profile);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Business")]
    public async Task<ActionResult<BusinessPrivateProfileResponse>> GetMyProfile(CancellationToken ct)
    {
        var businessProfileId = userContext.BusinessProfileId;
        if (!businessProfileId.HasValue)
        {
            return BadRequest(new { message = "User is not associated with a business profile." });
        }

        var profile = await businessService.GetMyProfileAsync(businessProfileId.Value, ct);
        return Ok(profile);
    }

    [HttpPut("me")]
    [Authorize(Roles = "Business")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateBusinessProfileRequest request, CancellationToken ct)
    {
        var businessProfileId = userContext.BusinessProfileId;
        if (!businessProfileId.HasValue)
        {
            return BadRequest(new { message = "User is not associated with a business profile." });
        }

        await businessService.UpdateMyProfileAsync(businessProfileId.Value, request, ct);
        return Ok();
    }

    [HttpGet("me/reviews")]
    public async Task<IActionResult> GetMyReviews(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var result = await reviewQueries.GetMyBusinessReviewsAsync(
            userId.Value,
            request,
            cancellationToken);

        return Ok(result);
    }
}
