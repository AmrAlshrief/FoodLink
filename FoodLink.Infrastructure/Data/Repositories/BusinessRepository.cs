using FoodLink.Application.Features.Businesses;
using FoodLink.Application.Features.Businesses.Interfaces;
using FoodLink.Domain.Entities.Profiles;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class BusinessRepository(AppDbContext dbContext) : IBusinessRepository
{
    public async Task<BusinessProfile?> GetByIdAsync(Guid businessProfileId, CancellationToken ct = default)
    {
        return await dbContext.BusinessProfiles
            .Include(b => b.User)
            .FirstOrDefaultAsync(b => b.Id == businessProfileId, ct);
    }

    public Task UpdateAsync(BusinessProfile business, CancellationToken ct = default)
    {
        return Task.CompletedTask;
    }
}
