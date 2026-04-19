using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class BusinessProfileRepository(AppDbContext dbContext) : IBusinessProfileRepository
{
    public async Task<BusinessProfile?> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return await dbContext.BusinessProfiles
            .FirstOrDefaultAsync(b => b.UserId == userId, cancellationToken);
    }

    public void Add(BusinessProfile profile)
    {
        dbContext.BusinessProfiles.Add(profile);
    }
}