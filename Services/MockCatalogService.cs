using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarPoolIndia.Models;

namespace CarPoolIndia.Services;

public class MockCatalogService : ICatalogService
{
    private readonly List<PoolCampaign> _seededPools;
    private readonly List<BuyerBooking> _userBookings;

    public MockCatalogService()
    {
        _seededPools = InitializeSeededPools();
        _userBookings = new List<BuyerBooking>
        {
            new BuyerBooking
            {
                BookingId = "CPI-2410-8842",
                PoolId = "pool-creta-blr",
                BuyerName = "Vikram Sharma",
                PhoneNumber = "+91 98450 12345",
                CityRto = "KA-01 (Koramangala, Bengaluru)",
                SelectedVariant = "SX (O) Diesel AT",
                SelectedColor = "Ranger Khaki with Abyss Black Roof",
                PaymentStatus = PaymentStatus.Paid,
                BookingDate = DateTime.UtcNow.AddDays(-3),
                VehicleName = "Hyundai Creta SX (O) Diesel AT",
                VehicleImageUrl = "https://images.unsplash.com/photo-1549399542-7e3f8b79c341?auto=format&fit=crop&w=800&q=80",
                TokenAmountPaidINR = 5000m,
                DealershipStatusNote = "Quorum at 70%. Target: 20 buyers. Admin actively collating allocation with Trident Hyundai."
            }
        };
    }

    public Task<List<PoolCampaign>> GetActivePoolsAsync()
    {
        return Task.FromResult(_seededPools.ToList());
    }

    public Task<PoolCampaign?> GetPoolByIdAsync(string poolId)
    {
        var pool = _seededPools.FirstOrDefault(p => p.Id == poolId);
        return Task.FromResult(pool);
    }

    public Task<BuyerBooking> CommitTokenBookingAsync(
        string poolId,
        string variant,
        string color,
        string buyerName,
        string phone,
        string cityRto)
    {
        var pool = _seededPools.FirstOrDefault(p => p.Id == poolId);
        if (pool != null)
        {
            pool.CurrentBuyers++;
            if (pool.CurrentBuyers >= pool.MinBuyers)
            {
                pool.Status = PoolStatus.Locked;
            }
        }

        var booking = new BuyerBooking
        {
            BookingId = $"CPI-{DateTime.UtcNow:yyMM}-{new Random().Next(1000, 9999)}",
            PoolId = poolId,
            BuyerName = buyerName,
            PhoneNumber = phone,
            CityRto = cityRto,
            SelectedVariant = variant,
            SelectedColor = color,
            PaymentStatus = PaymentStatus.Paid,
            BookingDate = DateTime.UtcNow,
            VehicleName = pool?.CarModel.FullDisplayName ?? "Booked Vehicle",
            VehicleImageUrl = pool?.CarModel.ImageUrl ?? string.Empty,
            TokenAmountPaidINR = 5000m,
            DealershipStatusNote = pool?.Status == PoolStatus.Locked
                ? "Quorum reached! Deal locked. Offline dealer allocation in progress."
                : "Commitment received! Tracking quorum progress towards group discount."
        };

        _userBookings.Insert(0, booking);
        return Task.FromResult(booking);
    }

    public Task<List<BuyerBooking>> GetUserBookingsAsync()
    {
        return Task.FromResult(_userBookings.ToList());
    }

    private static List<PoolCampaign> InitializeSeededPools()
    {
        return new List<PoolCampaign>
        {
            // 1. Hyundai Creta SX (O) Diesel AT
            new PoolCampaign
            {
                Id = "pool-creta-blr",
                TargetCity = "Bengaluru, Karnataka (KA RTO)",
                MinBuyers = 20,
                CurrentBuyers = 14,
                TargetDiscountPercent = 6.0,
                BaseExShowroomPrice = 2015000m,
                TokenAmountINR = 5000m,
                ExpiryDate = DateTime.UtcNow.AddDays(5).AddHours(14),
                Status = PoolStatus.Active,
                OfferPerks = "6% Group Discount + Complimentary 1st Year Zero-Dep Insurance (Worth ₹48,000)",
                CarModel = new CarModel
                {
                    Brand = "Hyundai",
                    ModelName = "Creta",
                    Segment = VehicleSegment.MidSizeSUV,
                    ImageUrl = "https://images.unsplash.com/photo-1549399542-7e3f8b79c341?auto=format&fit=crop&w=800&q=80",
                    StartingPriceINR = 1099900m,
                    AvailableVariants = new List<CarVariant>
                    {
                        new CarVariant { VariantName = "SX (O) Diesel AT (Flagship)", FuelType = "Diesel", Transmission = "6-Speed Automatic", ExShowroomPriceINR = 2015000m },
                        new CarVariant { VariantName = "SX (O) 1.5 Turbo Petrol DCT", FuelType = "Petrol", Transmission = "7-Speed DCT", ExShowroomPriceINR = 2000000m },
                        new CarVariant { VariantName = "SX Tech Diesel AT", FuelType = "Diesel", Transmission = "6-Speed Automatic", ExShowroomPriceINR = 1874000m }
                    }
                }
            },

            // 2. Tata Nexon Fearless Plus S
            new PoolCampaign
            {
                Id = "pool-nexon-pune-mum",
                TargetCity = "Pune & Mumbai (MH-12 / MH-02)",
                MinBuyers = 25,
                CurrentBuyers = 21,
                TargetDiscountPercent = 8.0,
                BaseExShowroomPrice = 1480000m,
                TokenAmountINR = 5000m,
                ExpiryDate = DateTime.UtcNow.AddDays(3).AddHours(8),
                Status = PoolStatus.Active,
                OfferPerks = "8% Bulk Direct Discount + Genuine Tata Accessories Pack (Worth ₹32,000)",
                CarModel = new CarModel
                {
                    Brand = "Tata",
                    ModelName = "Nexon",
                    Segment = VehicleSegment.CompactSUV,
                    ImageUrl = "https://images.unsplash.com/photo-1552519507-da3b142c6e3d?auto=format&fit=crop&w=800&q=80",
                    StartingPriceINR = 799990m,
                    AvailableVariants = new List<CarVariant>
                    {
                        new CarVariant { VariantName = "Fearless Plus S 1.2 Turbo DCA", FuelType = "Petrol", Transmission = "7-Speed Dual Clutch", ExShowroomPriceINR = 1480000m },
                        new CarVariant { VariantName = "Fearless Plus S 1.5 Diesel 6AMT", FuelType = "Diesel", Transmission = "6-Speed AMT", ExShowroomPriceINR = 1550000m },
                        new CarVariant { VariantName = "Creative Plus 1.2 Petrol DCA", FuelType = "Petrol", Transmission = "7-Speed Dual Clutch", ExShowroomPriceINR = 1260000m }
                    }
                }
            },

            // 3. Maruti Suzuki Brezza ZXi
            new PoolCampaign
            {
                Id = "pool-brezza-delhi",
                TargetCity = "Delhi NCR (DL & HR-26 Gurgaon)",
                MinBuyers = 15,
                CurrentBuyers = 9,
                TargetDiscountPercent = 5.0,
                BaseExShowroomPrice = 1254000m,
                TokenAmountINR = 5000m,
                ExpiryDate = DateTime.UtcNow.AddDays(7).AddHours(20),
                Status = PoolStatus.Active,
                OfferPerks = "5% Direct Cash Discount + 3-Year Extended Warranty & Underbody Coating",
                CarModel = new CarModel
                {
                    Brand = "Maruti Suzuki",
                    ModelName = "Brezza",
                    Segment = VehicleSegment.CompactSUV,
                    ImageUrl = "https://images.unsplash.com/photo-1503376780353-7e6692767b70?auto=format&fit=crop&w=800&q=80",
                    StartingPriceINR = 834000m,
                    AvailableVariants = new List<CarVariant>
                    {
                        new CarVariant { VariantName = "ZXi 1.5L Smart Hybrid 6AT", FuelType = "Petrol", Transmission = "6-Speed Automatic Torque Converter", ExShowroomPriceINR = 1254000m },
                        new CarVariant { VariantName = "ZXi 1.5L Smart Hybrid 5MT", FuelType = "Petrol", Transmission = "5-Speed Manual", ExShowroomPriceINR = 1114000m },
                        new CarVariant { VariantName = "ZXi CNG 1.5L 5MT", FuelType = "CNG + Petrol", Transmission = "5-Speed Manual", ExShowroomPriceINR = 1210000m }
                    }
                }
            },

            // 4. Mahindra Scorpio-N Z8 L
            new PoolCampaign
            {
                Id = "pool-scorpio-hyd",
                TargetCity = "Hyderabad, Telangana (TS RTO)",
                MinBuyers = 30,
                CurrentBuyers = 28,
                TargetDiscountPercent = 4.5,
                BaseExShowroomPrice = 2454000m,
                TokenAmountINR = 5000m,
                ExpiryDate = DateTime.UtcNow.AddDays(1).AddHours(12),
                Status = PoolStatus.Active, // Almost locked (28/30)
                OfferPerks = "Flat ₹60,000 Bulk Bonus + Fast-Track Priority Allocation & VIN Tagging",
                CarModel = new CarModel
                {
                    Brand = "Mahindra",
                    ModelName = "Scorpio-N",
                    Segment = VehicleSegment.FullSizeSUV,
                    ImageUrl = "https://images.unsplash.com/photo-1533473359331-0135ef1b58bf?auto=format&fit=crop&w=800&q=80",
                    StartingPriceINR = 1385000m,
                    AvailableVariants = new List<CarVariant>
                    {
                        new CarVariant { VariantName = "Z8 L 2.2 mHawk 4x4 Diesel AT (7-Str)", FuelType = "Diesel", Transmission = "6-Speed Automatic 4WD", ExShowroomPriceINR = 2454000m },
                        new CarVariant { VariantName = "Z8 L 2.2 mHawk 4x2 Diesel AT (7-Str)", FuelType = "Diesel", Transmission = "6-Speed Automatic", ExShowroomPriceINR = 2240000m },
                        new CarVariant { VariantName = "Z8 L 2.0 mStallion Turbo Petrol AT", FuelType = "Petrol", Transmission = "6-Speed Automatic", ExShowroomPriceINR = 2190000m }
                    }
                }
            }
        };
    }
}
