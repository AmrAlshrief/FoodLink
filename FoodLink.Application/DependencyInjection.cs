using FoodLink.Application.Features.Authentication.Services;
using FoodLink.Application.Features.Donations;
using FoodLink.Application.Features.Account.Services;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Features.Charity.Services;
using Microsoft.Extensions.DependencyInjection;

namespace FoodLink.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthService>();
        services.AddScoped<IDonationService, DonationService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IReservationService, ReservationService>();
        services.AddScoped<IAdminService, AdminService>();
        services.AddScoped<ICharityService, CharityService>();


        return services;
    }
}
