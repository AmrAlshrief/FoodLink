using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FoodLink.Infrastructure.Data.Repositories;

public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    
    public void Add(User user)
    {
        dbContext.Users.Add(user);
    }
    
    // ... insert other methods defined in your interface here ...
}
