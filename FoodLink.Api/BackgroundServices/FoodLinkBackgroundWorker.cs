using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Features.Reservation.Interfaces;
using FoodLink.Application.Features.Donation.Interfaces;

namespace FoodLink.Api.BackgroundServices;

public class FoodLinkBackgroundWorker(IServiceScopeFactory scopeFactory) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var reservationService =
                scope.ServiceProvider.GetRequiredService<IReservationService>();

            var donationService =
                scope.ServiceProvider.GetRequiredService<IDonationService>();

            await reservationService.HandleExpiredReservationsAsync(stoppingToken);
            await donationService.HandleExpiredDonationsAsync(stoppingToken);

            await Task.Delay(TimeSpan.FromMinutes(10), stoppingToken);
        }
    }
}