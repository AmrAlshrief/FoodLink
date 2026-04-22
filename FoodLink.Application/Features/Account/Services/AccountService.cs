using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Account.Dtos;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Application.Features.Account.Services;

public class AccountService(
    IUserRepository userRepository,
    IImageService imageService,
    IUserContext userContext,
    IUnitOfWork unitOfWork) : IAccountService
{
    public async Task<GetProfileResponse> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        var userId = userContext.UserId 
            ?? throw new DomainException("User is not authenticated.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new DomainException("User not found.");

        return new GetProfileResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Phone,
            user.Role.ToString(),
            user.ProfileImage
        );
    }

    public async Task<UpdateProfileResponse> UpdateProfileAsync(
        UpdateProfileRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = userContext.UserId
            ?? throw new DomainException("User is not authenticated.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new DomainException("User not found.");

        // Update fields if provided
        if (!string.IsNullOrWhiteSpace(request.Name))
            user.UpdateName(request.Name);

        if (!string.IsNullOrWhiteSpace(request.Phone))
            user.UpdatePhone(request.Phone);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateProfileResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Phone,
            user.ProfileImage,
            "Profile updated successfully."
        );
    }

    public async Task<UpdateProfileResponse> UpdateProfileImageAsync(
        UpdateProfileImageRequest request,
        CancellationToken cancellationToken = default)
    {
        var userId = userContext.UserId
            ?? throw new DomainException("User is not authenticated.");

        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new DomainException("User not found.");

        // Upload new image
        var profileImageUrl = await imageService.UploadImageAsync(
            request.ProfileImage,
            request.ProfileImageFileName ?? $"{Guid.NewGuid()}.jpg");

        user.SetProfileImage(profileImageUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return new UpdateProfileResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Phone,
            user.ProfileImage,
            "Profile image updated successfully."
        );
    }
}
