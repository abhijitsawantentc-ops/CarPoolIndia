using System.Collections.Generic;
using System.Threading.Tasks;
using CarPoolIndia.Models;
using Refit;

namespace CarPoolIndia.Services;

/// <summary>
/// Refit REST API interface for CarPool India catalog and booking endpoints.
/// Used in production or when pointing to an active backend.
/// </summary>
public interface ICatalogApi
{
    [Get("/api/v1/pools")]
    Task<List<PoolCampaign>> GetActivePoolsAsync();

    [Get("/api/v1/pools/{id}")]
    Task<PoolCampaign> GetPoolByIdAsync(string id);

    [Post("/api/v1/bookings/commit")]
    Task<BuyerBooking> CommitTokenAsync([Body] BuyerBooking booking);

    [Get("/api/v1/bookings/user/{phoneNumber}")]
    Task<List<BuyerBooking>> GetBookingsByPhoneAsync(string phoneNumber);
}
