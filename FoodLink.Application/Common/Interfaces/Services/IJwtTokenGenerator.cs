using FoodLink.Domain.Entities;

namespace FoodLink.Application.Common.Interfaces.Services;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, Guid? businessProfileId = null, Guid? charityProfileId = null);
}