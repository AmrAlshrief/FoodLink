using System.Security.Claims;
using FoodLink.Application.Common.Interfaces;

namespace FoodLink.Api.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid? UserId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public Guid? BusinessProfileId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue("businessProfileId");

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }

    public Guid? CharityProfileId
    {
        get
        {
            var value = httpContextAccessor.HttpContext?.User.FindFirstValue("charityProfileId");

            return Guid.TryParse(value, out var id) ? id : null;
        }
    }
}