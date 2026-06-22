using FoodLink.Application.Features.Businesses.DTOs;

namespace FoodLink.Application.Features.Businesses;

public interface IBusinessService
{
    Task<BusinessPublicProfileResponse> GetPublicProfileAsync(Guid businessProfileId, CancellationToken ct = default);
    Task<BusinessPrivateProfileResponse> GetMyProfileAsync(Guid businessProfileId, CancellationToken ct = default);
    Task UpdateMyProfileAsync(Guid businessProfileId, UpdateBusinessProfileRequest request, CancellationToken ct = default);
}
