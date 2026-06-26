using FoodLink.Application.Features.Authentication.Interfaces;
using FoodLink.Application.Features.Authentication.Services;
using FoodLink.Application.Features.Donation.Interfaces;
using FoodLink.Application.Features.Donations;
using FoodLink.Application.Features.Account.Interfaces;
using FoodLink.Application.Features.Account.Services;
using FoodLink.Application.Features.Reservation.Interfaces;
using FoodLink.Application.Features.Reservation.Services;
using FoodLink.Application.Features.Dashboard.Admin.Interfaces;
using FoodLink.Application.Features.Dashboard.Admin.Services;
using FoodLink.Application.Features.Charities.Interfaces;
using FoodLink.Application.Features.Charities;
using FoodLink.Application.Features.Businesses.Interfaces;
using FoodLink.Application.Features.Businesses;
using FoodLink.Application.Features.Reviews.Interfaces;
using FoodLink.Application.Features.Reviews.Services;
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
        services.AddScoped<IBusinessService, BusinessService>();
        services.AddScoped<IReviewService, ReviewService>();

        return services;
    }
}
