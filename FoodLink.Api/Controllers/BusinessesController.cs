using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Businesses;
using FoodLink.Application.Features.Businesses.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BusinessesController(IBusinessService businessService, IUserContext userContext) : ControllerBase
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
        return NoContent();
    }
}
