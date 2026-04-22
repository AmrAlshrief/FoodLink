using FoodLink.Application.Features.Reservation.Dtos;

namespace FoodLink.Application.Common.Interfaces.Services;

public interface IReservationService
{
    Task<Guid> CreateReservationAsync(CreateReservationRequest request, CancellationToken cancellationToken = default);
    Task CancelReservationAsync(Guid reservationId, CancellationToken cancellationToken = default);
    Task MarkPickedUpAsync(Guid reservationId, CancellationToken cancellationToken = default);
    Task<List<ReservationResponse>> GetMyReservationsAsync(CancellationToken cancellationToken = default);
    Task HandleExpiredReservationsAsync(CancellationToken cancellationToken = default);
}