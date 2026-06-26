using FoodLink.Domain.Entities;

namespace FoodLink.Application.Features.Authentication.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, Guid? businessProfileId = null, Guid? charityProfileId = null);
}
