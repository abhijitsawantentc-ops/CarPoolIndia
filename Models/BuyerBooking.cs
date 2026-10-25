using System;

namespace CarPoolIndia.Models;

public enum PaymentStatus
{
    Pending,
    Paid,       // ₹5,000 token received via UPI
    Refunded,   // If pool expires without quorum or buyer cancels before lock
    Failed
}

public class BuyerBooking
{
    public string BookingId { get; set; } = $"CPI-{DateTime.UtcNow:yyMM}-{new Random().Next(1000, 9999)}";
    public string PoolId { get; set; } = string.Empty;
    public string BuyerName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string CityRto { get; set; } = string.Empty; // e.g., "KA-01 (Koramangala RTO, Bengaluru)"
    public string SelectedVariant { get; set; } = string.Empty;
    public string SelectedColor { get; set; } = string.Empty;
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Paid;
    public DateTime BookingDate { get; set; } = DateTime.UtcNow;

    // Display metadata
    public string VehicleName { get; set; } = string.Empty;
    public string VehicleImageUrl { get; set; } = string.Empty;
    public decimal TokenAmountPaidINR { get; set; } = 5000m;
    public string UpiReferenceId { get; set; } = $"UPI{DateTime.UtcNow:mmss}{new Random().Next(100, 999)}";
    public string DealershipStatusNote { get; set; } = "Deal locked. Admin coordinating allocation with regional authorized dealership.";
}
