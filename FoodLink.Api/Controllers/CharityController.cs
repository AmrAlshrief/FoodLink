using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Charities.Interfaces;
using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Application.Features.Charities.DTOs;
using FoodLink.Application.Common.Models.Pagination;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CharityController(ICharityService charityService, IReviewQueries reviewQueries, IUserContext userContext) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CharityPublicProfileResponse>> GetPublicProfile(Guid id, CancellationToken ct)
    {
        var profile = await charityService.GetPublicProfileAsync(id, ct);
        return Ok(profile);
    }

    [HttpGet("me")]
    [Authorize(Roles = "Charity")]
    public async Task<ActionResult<CharityPrivateProfileResponse>> GetMyProfile(CancellationToken ct)
    {
        var charityProfileId = userContext.CharityProfileId;
        if (!charityProfileId.HasValue)
        {
            return BadRequest(new { message = "User is not associated with a charity profile." });
        }

        var profile = await charityService.GetMyProfileAsync(charityProfileId.Value, ct);
        return Ok(profile);
    }

    [HttpPut("me")]
    [Authorize(Roles = "Charity")]
    public async Task<IActionResult> UpdateMyProfile([FromBody] UpdateCharityProfileRequest request, CancellationToken ct)
    {
        var charityProfileId = userContext.CharityProfileId;
        if (!charityProfileId.HasValue)
        {
            return BadRequest(new { message = "User is not associated with a charity profile." });
        }

        await charityService.UpdateMyProfileAsync(charityProfileId.Value, request, ct);
        return Ok();
    }

    [HttpGet("me/reviews")]
    [Authorize(Roles = "Charity")]
    public async Task<IActionResult> GetMyReviews(
        [FromQuery] PaginationRequest request,
        CancellationToken cancellationToken)
    {
        var userId = userContext.UserId;
        if (!userId.HasValue)
        {
            return Unauthorized();
        }

        var result = await reviewQueries.GetMyCharityReviewsAsync(
            userId.Value,
            request,
            cancellationToken);

        return Ok(result);
    }
}
