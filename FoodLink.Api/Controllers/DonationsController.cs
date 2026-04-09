using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Donations.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
//[Authorize]
public class DonationsController(IDonationService donationService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDonation([FromBody] CreateDonationRequest request)
    {
        var donationId = await donationService.CreateDonationAsync(request);
        var donation = await donationService.GetDonationByIdAsync(donationId);
        return CreatedAtAction(nameof(GetDonation), new { id = donationId }, donation);
    }

    [HttpGet]
    public async Task<IActionResult> GetActiveDonations()
    {
        var donations = await donationService.GetAllActiveDonationsAsync();
        return Ok(donations);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetDonation(Guid id)
    {
        var donation = await donationService.GetDonationByIdAsync(id);
        if (donation == null) return NotFound();
        return Ok(donation);
    }
}