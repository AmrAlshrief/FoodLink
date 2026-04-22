using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Reservation.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Charity")]
    public async Task<IActionResult> CreateReservation([FromBody] CreateReservationRequest request)
    {
        var id = await reservationService.CreateReservationAsync(request);
        return Ok(id);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Charity")]
    public async Task<IActionResult> CancelReservation(Guid id)
    {
        await reservationService.CancelReservationAsync(id);
        return NoContent();
    }

    [HttpPut("{id}/pickup")]
    [Authorize(Roles = "Business")]
    public async Task<IActionResult> MarkPickedUp(Guid id)
    {
        await reservationService.MarkPickedUpAsync(id);
        return NoContent();
    }

    [HttpGet("me")]
    [Authorize(Roles = "Charity")]
    public async Task<IActionResult> GetMyReservations()
    {
        var result = await reservationService.GetMyReservationsAsync();
        return Ok(result);
    }
}