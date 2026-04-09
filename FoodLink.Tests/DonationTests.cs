using System;
using System.Linq;
using FoodLink.Domain.Common.Exceptions;
using FoodLink.Domain.Entities;
using Xunit;

namespace FoodLink.Tests;

public class DonationTests
{
    [Fact]
    public void CreateDonation_WithFutureExpiry_ShouldCreateDonation()
    {
        var businessId = Guid.NewGuid();
        var expiry = DateTime.UtcNow.AddDays(7);

        var donation = new Donation(businessId, "Vegetable Donation", "Fresh produce", expiry);

        Assert.Equal(businessId, donation.BusinessProfileId);
        Assert.Equal("Vegetable Donation", donation.Title);
        Assert.Equal("Fresh produce", donation.Description);
        Assert.Equal(expiry, donation.ExpiryDate);
        Assert.Equal(0, donation.Items.Count);
        Assert.Equal("Available", donation.Status.ToString());
    }

    [Fact]
    public void CreateDonation_WithPastExpiry_ShouldThrowDomainException()
    {
        var businessId = Guid.NewGuid();
        var expiry = DateTime.UtcNow.AddDays(-1);

        Assert.Throws<DomainException>(() => new Donation(businessId, "Expired Donation", "Old food", expiry));
    }

    [Fact]
    public void AddItem_ShouldAddDonationItem()
    {
        var donation = new Donation(Guid.NewGuid(), "Food Donation", "Lots of food", DateTime.UtcNow.AddDays(2));

        donation.AddItem("Carrots", 10, "kg");

        Assert.Single(donation.Items);
        var item = donation.Items.First();
        Assert.Equal("Carrots", item.Name);
        Assert.Equal(10, item.TotalQuantity);
        Assert.Equal("kg", item.Unit);
        Assert.Equal(10, item.AvailableQuantity);
    }

    [Fact]
    public void ReserveQuantity_WithValidAmount_ShouldReduceAvailableQuantity()
    {
        var donation = new Donation(Guid.NewGuid(), "Food Donation", "Fresh vegetables", DateTime.UtcNow.AddDays(2));
        donation.AddItem("Tomatoes", 15, "kg");
        var item = donation.Items.First();

        item.ReserveQuantity(5);

        Assert.Equal(5, item.ReservedQuantity);
        Assert.Equal(10, item.AvailableQuantity);
    }

    [Fact]
    public void ReserveQuantity_WithTooLargeAmount_ShouldThrowDomainException()
    {
        var donation = new Donation(Guid.NewGuid(), "Food Donation", "Fresh vegetables", DateTime.UtcNow.AddDays(2));
        donation.AddItem("Onions", 8, "kg");
        var item = donation.Items.First();

        Assert.Throws<DomainException>(() => item.ReserveQuantity(10));
    }
}
