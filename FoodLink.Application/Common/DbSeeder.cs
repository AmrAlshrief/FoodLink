using Microsoft.Extensions.DependencyInjection;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Domain.Enums;
using FoodLink.Domain.Entities;
namespace FoodLink.Application.Common;
public static class DbSeeder
{
    public static async Task SeedAdminAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var existingAdmin = await userRepository.GetByEmailAsync("admin@foodlink.com");

        if (existingAdmin is not null)
            return;

        var admin = User.Create(
            "Admin",
            "admin@foodlink.com",
            passwordHasher.Hash("Admin123"),
            UserRole.Admin,
            "0000000000"
        );

        userRepository.Add(admin);
        await unitOfWork.SaveChangesAsync();
    }
}