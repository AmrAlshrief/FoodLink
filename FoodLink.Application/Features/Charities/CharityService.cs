using FoodLink.Application.Common.Interfaces.Repositories;
using FoodLink.Application.Features.Charities.DTOs;
using FoodLink.Application.Features.Charities.Interfaces;
using FoodLink.Domain.Common.Exceptions;

namespace FoodLink.Application.Features.Charities;

public class CharityService(
    ICharityRepository charityRepository,
    ICharityQueries charityQueries,
    IUnitOfWork unitOfWork) : ICharityService
{
    public async Task<CharityPublicProfileResponse> GetPublicProfileAsync(Guid charityProfileId, CancellationToken ct = default)
    {
        return await charityQueries.GetPublicProfileAsync(charityProfileId, ct)
            ?? throw new NotFoundException("Charity profile not found.");
    }

    public async Task<CharityPrivateProfileResponse> GetMyProfileAsync(Guid charityProfileId, CancellationToken ct = default)
    {
        return await charityQueries.GetPrivateProfileAsync(charityProfileId, ct)
            ?? throw new NotFoundException("Charity profile not found.");
    }

    public async Task UpdateMyProfileAsync(Guid charityProfileId, UpdateCharityProfileRequest request, CancellationToken ct = default)
    {
        var charity = await charityRepository.GetByIdAsync(charityProfileId, ct)
            ?? throw new NotFoundException("Charity profile not found.");

        if (charity.User.IsSuspended)
            throw new UnauthorizedAccessException("Suspended users cannot update their profile.");

        // Validations
        if (request.Name != null)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                throw new DomainException("Name cannot be empty.");
            if (request.Name.Length > 100)
                throw new DomainException("Name must not exceed 100 characters.");
            
            charity.User.UpdateName(request.Name);
        }

        if (request.Phone != null)
        {
            if (string.IsNullOrWhiteSpace(request.Phone))
                throw new DomainException("Phone cannot be empty.");
            if (!System.Text.RegularExpressions.Regex.IsMatch(request.Phone, @"^\+?[0-9\s\-()]{7,20}$"))
                throw new DomainException("Phone number format is invalid.");
            
            charity.User.UpdatePhone(request.Phone);
        }

        if (request.Address != null)
        {
            if (string.IsNullOrWhiteSpace(request.Address))
                throw new DomainException("Address cannot be empty.");
            if (request.Address.Length > 200)
                throw new DomainException("Address must not exceed 200 characters.");
        }

        if (request.OrganizationName != null)
        {
            if (string.IsNullOrWhiteSpace(request.OrganizationName))
                throw new DomainException("Organization name cannot be empty.");
            if (request.OrganizationName.Length > 150)
                throw new DomainException("Organization name must not exceed 150 characters.");
        }

        if (request.ProfileImageUrl != null)
        {
            if (!string.IsNullOrWhiteSpace(request.ProfileImageUrl) && !Uri.IsWellFormedUriString(request.ProfileImageUrl, UriKind.Absolute))
                throw new DomainException("Profile image URL is invalid.");
            
            charity.User.SetProfileImage(request.ProfileImageUrl);
        }

        var orgName = request.OrganizationName ?? charity.Name;
        var address = request.Address ?? charity.Address;
        charity.UpdateDetails(orgName, address);

        await charityRepository.UpdateAsync(charity, ct);
        await unitOfWork.SaveChangesAsync(ct);
    }
}
