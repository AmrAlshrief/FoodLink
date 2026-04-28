using FoodLink.Application.Features.Dashboard.Admin.DTOs;
namespace FoodLink.Application.Common.Interfaces.Services;
public interface IAdminService
{
    // Users
    Task<List<UserResponse>> GetAllUsersAsync();
    /*
    Task BlockUserAsync(Guid userId);
    Task UnblockUserAsync(Guid userId);

    // Donations
    Task<List<DonationResponse>> GetAllDonationsAsync();
    Task DeleteDonationAsync(Guid donationId);

    // Reservations
    Task<List<ReservationResponse>> GetAllReservationsAsync();
    Task ForceCancelReservationAsync(Guid reservationId);
    */

    // System
    Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken = default);



    
}