using System.Collections.Generic;
using System.Threading.Tasks;
using CarPoolIndia.Models;

namespace CarPoolIndia.Services;

public interface ICatalogService
{
    Task<List<PoolCampaign>> GetActivePoolsAsync();
    Task<PoolCampaign?> GetPoolByIdAsync(string poolId);
    Task<BuyerBooking> CommitTokenBookingAsync(string poolId, string variant, string color, string buyerName, string phone, string cityRto);
    Task<List<BuyerBooking>> GetUserBookingsAsync();
}
