using FoodLink.Application.Features.Account.Dtos;
using FoodLink.Application.Features.Account.Services;
using FoodLink.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpGet("profile")]
    public async Task<IActionResult> GetProfile(CancellationToken cancellationToken)
    {
        var profile = await accountService.GetProfileAsync(cancellationToken);
        return Ok(profile);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromBody] UpdateProfileHttpRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new UpdateProfileRequest(
            httpRequest.Name,
            httpRequest.Phone
        );

        var result = await accountService.UpdateProfileAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpPost("profile-image")]
    public async Task<IActionResult> UpdateProfileImage(
        [FromForm] UpdateProfileImageHttpRequest httpRequest,
        CancellationToken cancellationToken)
    {
        var request = new UpdateProfileImageRequest(
            httpRequest.ProfileImage.OpenReadStream(),
            httpRequest.ProfileImage.FileName ?? $"{Guid.NewGuid()}.jpg"
        );

        var result = await accountService.UpdateProfileImageAsync(request, cancellationToken);
        return Ok(result);
    }
}
