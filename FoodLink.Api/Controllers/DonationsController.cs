using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Donations.Dtos;
using FoodLink.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DonationsController(IDonationService donationService) : ControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Business")]
    public async Task<IActionResult> CreateDonation([FromForm] CreateDonationHttpRequest httpRequest)
    {
        var request = new CreateDonationRequest
        {
            Title = httpRequest.Title,
            Description = httpRequest.Description,
            ExpiryDate = httpRequest.ExpiryDate,
            Image = httpRequest.Image?.OpenReadStream(),
            ImageFileName = httpRequest.Image?.FileName
        };

        var donationId = await donationService.CreateDonationAsync(request);
        var donation = await donationService.GetDonationByIdAsync(donationId);

        return CreatedAtAction(nameof(GetDonation), new { id = donationId }, donation);
    }

    [HttpPost("{donationId}/items")]
    [Authorize(Roles = "Business")]
    public async Task<IActionResult> AddItem(Guid donationId, [FromForm] AddDonationItemHttpRequest httpRequest)
    {
        var request = new AddDonationItemRequest
        {
            Name = httpRequest.Name,
            Quantity = httpRequest.Quantity,
            Unit = httpRequest.Unit,
            Image = httpRequest.Image?.OpenReadStream(),
            ImageFileName = httpRequest.Image?.FileName
        };

        await donationService.AddItemAsync(donationId, request);
        return NoContent();
    }

    [HttpGet]
    [Authorize(Roles = "Business")]
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

    [HttpPut("{id}")]
    //[Authorize(Roles = "Business")]
    public async Task<IActionResult> UpdateDonation(Guid id, [FromForm] UpdateDonationHttpRequest httpRequest)
    {
        if (id != httpRequest.Id)
            return BadRequest("Route id does not match request id.");

        var request = new UpdateDonationRequest
        {
            Id = httpRequest.Id,
            Title = httpRequest.Title,
            Description = httpRequest.Description,
            ExpiryDate = httpRequest.ExpiryDate,
            Image = httpRequest.Image?.OpenReadStream(),
            ImageFileName = httpRequest.Image?.FileName
        };

        await donationService.UpdateDonationAsync(request);
        return NoContent();
    }
}