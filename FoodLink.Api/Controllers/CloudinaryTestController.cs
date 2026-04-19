using FoodLink.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodLink.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CloudinaryTestController(IImageService imageService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<IActionResult> UploadImage()
    {
        var file = HttpContext.Request.Form.Files["file"];
        if (file == null || file.Length == 0)
        {
            return BadRequest(new { Message = "Please provide an image file in the 'file' form field." });
        }

        await using var stream = file.OpenReadStream();
        var imageUrl = await imageService.UploadImageAsync(stream, file.FileName);

        return Ok(new { Url = imageUrl });
    }
}
