using FoodLink.Application.Features.Account.Dtos;

namespace FoodLink.Application.Features.Account.Services;

public interface IAccountService
{
    Task<GetProfileResponse> GetProfileAsync(CancellationToken cancellationToken = default);
    Task<UpdateProfileResponse> UpdateProfileAsync(UpdateProfileRequest request, CancellationToken cancellationToken = default);
    Task<UpdateProfileResponse> UpdateProfileImageAsync(UpdateProfileImageRequest request, CancellationToken cancellationToken = default);
}
