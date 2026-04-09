namespace FoodLink.Application.Common.Interfaces;

public interface IImageService
{
    Task<string> UploadImageAsync(Stream imageStream, string fileName);
}
