using FoodLink.Application.Features.Businesses.DTOs;

namespace FoodLink.Application.Features.Businesses.Interfaces;

public interface IBusinessQueries
{
    Task<BusinessPublicProfileResponse?> GetPublicProfileAsync(Guid businessProfileId, CancellationToken ct = default);
    Task<BusinessPrivateProfileResponse?> GetPrivateProfileAsync(Guid businessProfileId, CancellationToken ct = default);
}
