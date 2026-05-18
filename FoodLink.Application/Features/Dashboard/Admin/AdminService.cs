using FoodLink.Domain.Entities;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Common.Interfaces.Services.Queries;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Dashboard.Admin.DTOs;
using FoodLink.Application.Common.Models.Pagination;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Application.Features.Dashboard.Admin.Services;

public class AdminService(IUserRepository userRepository,
                          IDonationRepository donationRepository,
                          IReservationRepository reservationRepository,
                          IReservationService reservationService,
                          IAdminQueries adminQueries,
                          IUnitOfWork unitOfWork) : IAdminService
{

    public async Task<List<UserResponse>> GetAllUsersAsync()
    {
        var users = await userRepository.GetAllAsync();

        return users.Select(u => new UserResponse
        {
            Id = u.Id,
            Name = u.Name,
            Email = u.Email,
            Role = u.Role.ToString()
        }).ToList();
    }

    public async Task SuspendUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new DomainException("User not found.");

        if (user.IsAdmin())
            throw new DomainException("Cannot suspend admin user.");

        user.Suspend();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task ReactivateUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var user = await userRepository.GetByIdAsync(userId, cancellationToken)
            ?? throw new DomainException("User not found.");

        user.Reactivate();

        await unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task<AdminDashboardStatsResponse> GetDashboardStatsAsync(
    CancellationToken cancellationToken = default)
    {
        return await adminQueries.GetDashboardStatsAsync(cancellationToken);
    }

    public async Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken = default)
    {
        await reservationService.HandleExpiredReservationsAsync(cancellationToken);
    }

    public async Task<PagedResponse<AdminCharityResponse>> GetCharitiesAsync(
    AdminFilterRequest filter,
    CancellationToken cancellationToken = default)
    {
        return await adminQueries.GetCharitiesAsync(filter, cancellationToken);
    }

    public async Task<AdminCharityResponse> GetCharityByIdAsync(
    Guid charityId,
    CancellationToken cancellationToken = default)
    {
        var charity = await adminQueries.GetCharityByIdAsync(charityId, cancellationToken);

        if (charity is null)
            throw new DomainException("Charity not found.");

        return charity;
    }

    public async Task<PagedResponse<AdminBusinessResponse>> GetBusinessesAsync(
        AdminFilterRequest filter,
        CancellationToken cancellationToken = default)
    {
        return await adminQueries.GetBusinessesAsync(filter, cancellationToken);
    }

    public async Task<AdminBusinessResponse> GetBusinessByIdAsync(
        Guid businessId,
        CancellationToken cancellationToken = default)
    {
        var business = await adminQueries.GetBusinessByIdAsync(businessId, cancellationToken);

        if (business is null)
            throw new DomainException("Business not found.");

        return business;
    }

    public async Task<PagedResponse<AdminUserResponse>> GetUsersAsync(
    AdminUserFilterRequest request,
    CancellationToken cancellationToken = default)
    {
        return await adminQueries.GetUsersAsync(request, cancellationToken);
    }
}