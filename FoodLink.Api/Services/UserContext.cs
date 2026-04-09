using System.Security.Claims;
using FoodLink.Application.Common.Interfaces;

namespace FoodLink.Api.Services;

public class UserContext(IHttpContextAccessor httpContextAccessor) : IUserContext
{
    public Guid? UserId
    {
        get
        {
            var userIdStr = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            if (Guid.TryParse(userIdStr, out var userId))
            {
                return userId;
            }

            return null;
        }
    }
}
