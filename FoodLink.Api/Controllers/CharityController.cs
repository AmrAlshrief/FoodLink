using FoodLink.Application.Features.Charity.Dtos;
using FoodLink.Application.Features.Charity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CharityController(ICharityService charityService) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<ActionResult<List<CharityResponse>>> GetAllCharities(CancellationToken cancellationToken)
    {
        var charities = await charityService.GetAllCharitiesAsync(cancellationToken);
        return Ok(charities);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CharityResponse>> GetCharityById(Guid id, CancellationToken cancellationToken)
    {
        var charity = await charityService.GetCharityByIdAsync(id, cancellationToken);
        
        if (charity == null)
        {
            return NotFound($"Charity with ID {id} not found.");
        }

        return Ok(charity);
    }

    [HttpGet("user/{userId:guid}")]
    public async Task<ActionResult<CharityResponse>> GetCharityByUserId(Guid userId, CancellationToken cancellationToken)
    {
        var charity = await charityService.GetCharityByUserIdAsync(userId, cancellationToken);
        
        if (charity == null)
        {
            return NotFound($"Charity associated with User ID {userId} not found.");
        }

        return Ok(charity);
    }
}
