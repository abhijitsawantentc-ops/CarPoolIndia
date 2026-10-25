using System.Collections.Generic;

namespace CarPoolIndia.Models;

public enum VehicleSegment
{
    CompactSUV,
    MidSizeSUV,
    FullSizeSUV,
    Sedan,
    EV
}

public class CarVariant
{
    public string VariantName { get; set; } = string.Empty;
    public string FuelType { get; set; } = string.Empty; // Petrol, Diesel, CNG, Electric
    public string Transmission { get; set; } = string.Empty; // Manual, Automatic, DCT, Torque Converter
    public decimal ExShowroomPriceINR { get; set; }

    public string FormattedPrice => $"₹{ExShowroomPriceINR / 100000m:0.00} Lakh";
}

public class CarModel
{
    public string Brand { get; set; } = string.Empty;
    public string ModelName { get; set; } = string.Empty;
    public VehicleSegment Segment { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
    public decimal StartingPriceINR { get; set; }
    public List<CarVariant> AvailableVariants { get; set; } = new();

    public string FullDisplayName => $"{Brand} {ModelName}";
    public string FormattedStartingPrice => $"Starting at ₹{StartingPriceINR / 100000m:0.00} Lakh";
}
