using FoodLink.Application.Features.Account.Dtos;
using FoodLink.Application.Features.Account.Services;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController(IAdminService adminService) : ControllerBase
{
    // [HttpGet("dashboard-stats")]
    // public async Task<IActionResult> GetDashboardStats(CancellationToken cancellationToken)
    // {
    //     var stats = await adminService.GetDashboardStatsAsync(cancellationToken);
    //     return Ok(stats);
    // }

    [HttpPost("reservations/process-expired")]
    //[Authorize(Roles = "Admin")]
    public async Task<IActionResult> ProcessExpiredReservations(CancellationToken cancellationToken)
    {
        await adminService.ProcessExpiredReservationsAsync(cancellationToken);
        return NoContent();
    }

    

}