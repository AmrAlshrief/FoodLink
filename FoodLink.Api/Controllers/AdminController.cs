using FoodLink.Application.Features.Account.Dtos;
using FoodLink.Application.Features.Account.Services;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Dashboard.Admin.DTOs;
using FoodLink.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{

    [HttpPost("reservations/process-expired")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProcessExpiredReservations(CancellationToken cancellationToken)
    {
        await adminService.ProcessExpiredReservationsAsync(cancellationToken);
        return NoContent();
    }

    [HttpPost("users/{userId}/suspend")]
    public async Task<IActionResult> SuspendUser(Guid userId, CancellationToken cancellationToken)
    {
        await adminService.SuspendUserAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpPost("users/{userId}/reactivate")]
    public async Task<IActionResult> ReactivateUser(Guid userId, CancellationToken cancellationToken)
    {
        await adminService.ReactivateUserAsync(userId, cancellationToken);
        return NoContent();
    }

    [HttpGet("dashboard/stats")]
    public async Task<IActionResult> GetDashboardStats(
        CancellationToken cancellationToken)
    {
        var result = await adminService
            .GetDashboardStatsAsync(cancellationToken);

        return Ok(result);
    }


    [HttpGet("charities")]
    public async Task<IActionResult> GetCharities(
        [FromQuery] AdminFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var result = await adminService
            .GetCharitiesAsync(filter, cancellationToken);

        return Ok(result);
    }

    [HttpGet("charities/{charityId}")]
    public async Task<IActionResult> GetCharity(
        Guid charityId,
        CancellationToken cancellationToken)
    {
        var result = await adminService
            .GetCharityByIdAsync(charityId, cancellationToken);

        return Ok(result);
    }

    [HttpGet("businesses")]
    public async Task<IActionResult> GetBusinesses(
        [FromQuery] AdminFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var result = await adminService
            .GetBusinessesAsync(filter, cancellationToken);

        return Ok(result);
    }

    [HttpGet("businesses/{businessId}")]
    public async Task<IActionResult> GetBusiness(
        Guid businessId,
        CancellationToken cancellationToken)
    {
        var result = await adminService
            .GetBusinessByIdAsync(businessId, cancellationToken);

        return Ok(result);
    }
    [HttpGet("users")]
    public async Task<IActionResult> GetUsers(
        [FromQuery] AdminUserFilterRequest request,
        CancellationToken cancellationToken)
    {
        var users = await adminService.GetUsersAsync(request, cancellationToken);
        return Ok(users);
    }
}