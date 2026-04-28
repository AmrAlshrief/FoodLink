using FoodLink.Domain.Entities;
using FoodLink.Application.Common.Interfaces;
using FoodLink.Application.Common.Interfaces.Services;
using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminService(IUserRepository userRepository,
                          IDonationRepository donationRepository,
                          IReservationRepository reservationRepository,
                          IReservationService reservationService,
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

    // public async Task BlockUserAsync(Guid userId)
    // {
    //     var user = await userRepository.GetByIdAsync(userId)
    //         ?? throw new DomainException("User not found");

    //     user.Block();
    //     await unitOfWork.SaveChangesAsync();
    // }

    public async Task ProcessExpiredReservationsAsync(CancellationToken cancellationToken = default)
    {
        await reservationService.HandleExpiredReservationsAsync(cancellationToken);
    }
    
}