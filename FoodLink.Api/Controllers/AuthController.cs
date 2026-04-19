using FoodLink.Application.Features.Authentication.Services;
using FoodLink.Application.Features.Authentication.DTOs;
using FoodLink.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthenticationService authService) : ControllerBase
{
    [HttpPost("register-business")]
    public async Task<IActionResult> RegisterBusiness(
        [FromForm] RegisterBusinessHttpRequest request,
        CancellationToken cancellationToken)
    {
        var appRequest = new RegisterBusinessRequest(
            request.Name,
            request.Email,
            request.Password,
            request.Phone,
            request.ProfileImage?.OpenReadStream(),
            request.ProfileImage?.FileName,
            request.BusinessName,
            request.Address,
            request.BusinessType
        );

        var result = await authService.RegisterBusinessAsync(appRequest, cancellationToken);
        return Ok(result);
    }

    [HttpPost("register-charity")]
    public async Task<IActionResult> RegisterCharity(
        [FromForm] RegisterCharityHttpRequest request,
        CancellationToken cancellationToken)
    {
        var appRequest = new RegisterCharityRequest(
            request.Name,
            request.Email,
            request.Password,
            request.Phone,
            request.ProfileImage?.OpenReadStream(),
            request.ProfileImage?.FileName,
            request.OrganizationName,
            request.LicenseNumber,
            request.Address
        );

        var result = await authService.RegisterCharityAsync(appRequest, cancellationToken);
        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(
        [FromBody] LoginRequest request,
        CancellationToken cancellationToken)
    {
        var result = await authService.LoginAsync(request, cancellationToken);
        return Ok(result);
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken)
    {
        var users = await authService.GetAllUsersAsync(cancellationToken);
        return Ok(users);
    }
}