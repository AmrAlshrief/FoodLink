using FoodLink.Application.Features.Charities.DTOs;

namespace FoodLink.Application.Features.Charities.Interfaces;

public interface ICharityService
{
    Task<CharityPublicProfileResponse> GetPublicProfileAsync(Guid charityProfileId, CancellationToken ct = default);
    Task<CharityPrivateProfileResponse> GetMyProfileAsync(Guid charityProfileId, CancellationToken ct = default);
    Task UpdateMyProfileAsync(Guid charityProfileId, UpdateCharityProfileRequest request, CancellationToken ct = default);
}
