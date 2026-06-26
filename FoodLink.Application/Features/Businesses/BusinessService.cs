using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Businesses.DTOs;
using FoodLink.Application.Features.Businesses.Interfaces;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Application.Features.Businesses;

public class BusinessService(
    IBusinessRepository businessRepository,
    IBusinessQueries businessQueries,
    IUnitOfWork unitOfWork) : IBusinessService
{
    public async Task<BusinessPublicProfileResponse> GetPublicProfileAsync(Guid businessProfileId, CancellationToken ct = default)
    {
        return await businessQueries.GetPublicProfileAsync(businessProfileId, ct)
            ?? throw new NotFoundException("Business profile not found.");
    }

    public async Task<BusinessPrivateProfileResponse> GetMyProfileAsync(Guid businessProfileId, CancellationToken ct = default)
    {
        return await businessQueries.GetPrivateProfileAsync(businessProfileId, ct)
            ?? throw new NotFoundException("Business profile not found.");
    }

    public async Task UpdateMyProfileAsync(Guid businessProfileId, UpdateBusinessProfileRequest request, CancellationToken ct = default)
    {
        var business = await businessRepository.GetByIdAsync(businessProfileId, ct)
            ?? throw new NotFoundException("Business profile not found.");

        if (business.User.IsSuspended)
            throw new UnauthorizedAccessException("Suspended users cannot update their profile.");

        // Validations
        if (request.Name != null)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new DomainException("Name cannot be empty.");
            if (request.Name.Length > 100)
                throw new DomainException("Name must not exceed 100 characters.");
            
            business.User.UpdateName(request.Name);
        }

        if (request.Phone != null)
        {
            if (string.IsNullOrWhiteSpace(request.Phone))
                throw new DomainException("Phone cannot be empty.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(request.Phone, @"^\+?[0-9\s\-()]{7,20}$"))
                throw new DomainException("Phone number format is invalid.");
            
            business.User.UpdatePhone(request.Phone);
        }

        if (request.Address != null)
        {
            if (string.IsNullOrWhiteSpace(request.Address))
                throw new DomainException("Address cannot be empty.");
            if (request.Address.Length > 200)
                throw new DomainException("Address must not exceed 200 characters.");
        }

        if (request.BusinessName != null)
        {
            if (string.IsNullOrWhiteSpace(request.BusinessName))
                throw new DomainException("Business name cannot be empty.");
            if (request.BusinessName.Length > 150)
                throw new DomainException("Business name must not exceed 150 characters.");
        }

        if (request.BusinessType != null)
        {
            if (string.IsNullOrWhiteSpace(request.BusinessType))
                throw new DomainException("Business type cannot be empty.");
        }

        if (request.ProfileImageUrl != null)
        {
            if (!string.IsNullOrWhiteSpace(request.ProfileImageUrl) && !Uri.IsWellFormedUriString(request.ProfileImageUrl, UriKind.Absolute))
                throw new DomainException("Profile image URL is invalid.");
            
            business.User.SetProfileImage(request.ProfileImageUrl);
        }

        var busName = request.BusinessName ?? business.BusinessName;
        var address = request.Address ?? business.Address;
        var busType = request.BusinessType ?? business.BusinessType;
        business.UpdateDetails(busName, address, busType);

        await businessRepository.UpdateAsync(business, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
