using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Reservation.Dtos;
using FoodLink.Application.Common.Interfaces.Services.Queries;
using FoodLink.Application.Common.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReservationController(IReservationService reservationService,
                                   IReservationQueries reservationQueries,
                                   IUserContext userContext) : ControllerBase
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
    [Authorize(Roles = "Charity,Admin")]
    public async Task<IActionResult> GetMyReservations()
    {
        var charityId = userContext.CharityProfileId
            ?? throw new Exception("User does not have a charity profile.");
        var result = await reservationQueries.GetMyReservationsAsync(charityId);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Business,Admin")]
    public async Task<IActionResult> ReservationsByDonationId(
        [FromQuery] ReservationFilterRequest filter
    )
    {
        var charityId = userContext.CharityProfileId
            ?? throw new Exception("User does not have a charity profile.");
        var result = await reservationQueries.GetReservationsAsync(charityId, filter);
        return Ok(result);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Business")]
    public async Task<IActionResult> Reservations(
        [FromQuery] Guid charityId,
        [FromQuery] ReservationFilterRequest filter)
    {
        
       
        var result = await reservationQueries.GetReservationsAsync(charityId, filter);
        return Ok(result);
    }

    [HttpGet("donations/{donationId}/reservations")]
    [Authorize(Roles = "Business,Admin")]
    public async Task<IActionResult> GetReservationsForDonation(
        Guid donationId,
        [FromQuery] ReservationFilterRequest filter,
        CancellationToken cancellationToken)
    {
        var businessId = userContext.BusinessProfileId
            ?? throw new Exception("User does not have a business profile.");

        var result = await reservationQueries.GetReservationsByDonationAsync(
            businessId,
            donationId,
            filter,
            cancellationToken);

        return Ok(result);
    }


}