using FoodLink.Application.Common.Interfaces.Repositories;

namespace FoodLink.Infrastructure.Data.Repositories;

public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
{
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return dbContext.SaveChangesAsync(cancellationToken);
    }
}
