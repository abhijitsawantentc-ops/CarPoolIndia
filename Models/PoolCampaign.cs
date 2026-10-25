using System;

namespace CarPoolIndia.Models;

public enum PoolStatus
{
    Active,      // Pool is open for buyer commitments (₹5,000 token)
    Locked,      // Quorum reached! Group deal locked with dealer
    Allocating,  // Dealer allocation & VIN tagging in progress by admin offline
    Completed    // Vehicles delivered to group members
}

public class PoolCampaign
{
    public string Id { get; set; } = Guid.NewGuid().ToString("N");
    public CarModel CarModel { get; set; } = new();
    public string TargetCity { get; set; } = string.Empty;
    public int MinBuyers { get; set; }
    public int CurrentBuyers { get; set; }
    public double TargetDiscountPercent { get; set; }
    public decimal BaseExShowroomPrice { get; set; }
    public decimal TokenAmountINR { get; set; } = 5000m; // Refundable UPI commitment deposit
    public DateTime ExpiryDate { get; set; }
    public PoolStatus Status { get; set; } = PoolStatus.Active;

    // Value-add dealership offer perks negotiated for group
    public string OfferPerks { get; set; } = string.Empty;

    // Helper calculated properties
    public double ProgressRatio => MinBuyers > 0 ? Math.Min(1.0, (double)CurrentBuyers / MinBuyers) : 0.0;
    public int ProgressPercentage => (int)Math.Round(ProgressRatio * 100);
    public int BuyersRemaining => Math.Max(0, MinBuyers - CurrentBuyers);
    public bool IsQuorumReached => CurrentBuyers >= MinBuyers;
    public TimeSpan TimeRemaining => ExpiryDate > DateTime.UtcNow ? ExpiryDate - DateTime.UtcNow : TimeSpan.Zero;

    public decimal EstimatedSavingsINR => Math.Round(BaseExShowroomPrice * ((decimal)TargetDiscountPercent / 100m));
    public string FormattedBasePrice => $"₹{BaseExShowroomPrice / 100000m:0.00} Lakh";
    public string FormattedSavings => $"Save ~₹{EstimatedSavingsINR / 1000m:0}k ({TargetDiscountPercent:0.#}%)";
    public string FormattedToken => $"₹{TokenAmountINR:N0}";
}
