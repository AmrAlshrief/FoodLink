using FoodLink.Application.Features.Dashboard.Admin.DTOs;
using FoodLink.Application.Common.Models.Pagination;
namespace FoodLink.Application.Common.Interfaces.Services;
public interface IAdminService
{
    // Users
    Task<List<UserResponse>> GetAllUsersAsync();
    Task SuspendUserAsync(Guid userId, CancellationToken cancellationToken = default);
    Task ReactivateUserAsync(Guid userId, CancellationToken cancellationToken = default);

    /*
   
    // Donations
    Task<List<DonationResponse>> GetAllDonationsAsync();
    Task DeleteDonationAsync(Guid donationId);

    // Reservations
    Task<List<ReservationResponse>> GetAllReservationsAsync();
    Task ForceCancelReservationAsync(Guid reservationId);
    */

    // System
    Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken = default);


    // Dashboard
    Task<AdminDashboardStatsResponse> GetDashboardStatsAsync(CancellationToken cancellationToken = default);
    
    Task<PagedResponse<AdminCharityResponse>> GetCharitiesAsync(
        AdminFilterRequest filter,
        CancellationToken cancellationToken = default);

    Task<AdminCharityResponse?> GetCharityByIdAsync(
        Guid charityId,
        CancellationToken cancellationToken = default);

    Task<PagedResponse<AdminBusinessResponse>> GetBusinessesAsync(
        AdminFilterRequest filter,
        CancellationToken cancellationToken = default);

    Task<AdminBusinessResponse> GetBusinessByIdAsync(
        Guid businessId,
        CancellationToken cancellationToken = default);
        
    Task<PagedResponse<AdminUserResponse>> GetUsersAsync(
        AdminUserFilterRequest request,
        CancellationToken cancellationToken = default);
}