using FoodLink.Infrastructure.Data;
using FoodLink.Infrastructure.Data.Interceptors;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Common.Interfaces.Services.Queries;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Infrastructure.Data.Queries;
using FoodLink.Infrastructure.Data.Repositories;
using FoodLink.Infrastructure.Services.Authentication;
using FoodLink.Infrastructure.Services.ImageUpload;
using FoodLink.Infrastructure.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FoodLink.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Database & Interceptors
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        services.AddScoped<AuditableEntityInterceptor>();

        services.AddDbContext<AppDbContext>((sp, options) =>
        {
            var interceptor = sp.GetRequiredService<AuditableEntityInterceptor>();
            options.UseSqlite(connectionString) // Using SQLite as requested
                   .AddInterceptors(interceptor);
        });

        // 2. JWT Authentication (This reads the token from headers)
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(jwtSettings["Secret"]!)),
                ClockSkew = TimeSpan.Zero
            };
        });
        
        services.AddAuthorization();

        // 3. Register Repositories
        services.AddScoped<IDonationRepository, DonationRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IBusinessProfileRepository, BusinessProfileRepository>();
        services.AddScoped<ICharityProfileRepository, CharityProfileRepository>();  
        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IReservationQueries, ReservationQueries>();

        // 4. Register Authentication Services
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        
        // 5. Register Image Service
        services.AddScoped<IImageService, CloudinaryService>();
        
        return services;
    }
}
