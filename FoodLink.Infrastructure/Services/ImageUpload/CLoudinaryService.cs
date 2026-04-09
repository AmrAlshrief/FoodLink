namespace FoodLink.Infrastructure.Services.ImageUpload;

using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using FoodLink.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

public class CloudinaryService : IImageService
{
    private readonly Cloudinary _cloudinary;

    public CloudinaryService(IConfiguration configuration)
    {
        var account = new Account(
            configuration["Cloudinary:CloudName"],
            configuration["Cloudinary:ApiKey"],
            configuration["Cloudinary:ApiSecret"]
        );

        _cloudinary = new Cloudinary(account);
    }

    public async Task<string> UploadImageAsync(Stream fileStream, string fileName)
    {
        var uploadParams = new ImageUploadParams
        {
            File = new FileDescription(fileName, fileStream)
        };

        var result = await _cloudinary.UploadAsync(uploadParams);

        if (result.StatusCode != System.Net.HttpStatusCode.OK)
            throw new Exception("Image upload failed");

        return result.SecureUrl.ToString();
    }
}