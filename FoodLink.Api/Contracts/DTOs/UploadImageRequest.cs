using Microsoft.AspNetCore.Http;

namespace FoodLink.Api.Contracts.DTOs;

public class UploadImageRequest
{
    public IFormFile? Image { get; set; }
}