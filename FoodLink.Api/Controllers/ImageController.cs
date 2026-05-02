using FoodLink.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FoodLink.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ImageController(IImageService imageService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> UploadImage([FromForm] UploadImageRequest request)
    {
        if (request.Image == null)
            return BadRequest("Image is required.");

        var imageUrl = await imageService.UploadImageAsync(
            request.Image.OpenReadStream(),
            request.Image.FileName);

        return Ok(new { imageUrl });
    }
}
