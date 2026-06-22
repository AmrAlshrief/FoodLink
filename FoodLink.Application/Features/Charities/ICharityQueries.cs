using FoodLink.Application.Features.Charities.DTOs;

namespace FoodLink.Application.Features.Charities;

public interface ICharityQueries
{
    Task<CharityPublicProfileResponse?> GetPublicProfileAsync(Guid charityProfileId, CancellationToken ct = default);
    Task<CharityPrivateProfileResponse?> GetPrivateProfileAsync(Guid charityProfileId, CancellationToken ct = default);
}
