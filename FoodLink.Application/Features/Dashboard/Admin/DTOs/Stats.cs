namespace FoodLink.Application.Features.Dashboard.Admin.DTOs;

public class AdminDashboardStatsResponse
{
    public int TotalCharities { get; set; }
    public int TotalBusinesses { get; set; }
    public int TotalDonations { get; set; }
    public int TotalReservations { get; set; }

    public int PendingReservations { get; set; }
    public int PickedUpReservations { get; set; }
    public int NoShowReservations { get; set; }

    public int ActiveDonations { get; set; }
    public int ExpiredDonations { get; set; }
}