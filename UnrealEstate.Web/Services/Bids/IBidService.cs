using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Bids;

namespace UnrealEstate.Web.Services.Bids
{
    public interface IBidService
    {
        Task<Bid> GetBidById(int Id);

        Task<List<BidViewModel>> GetBidsByListingId(int listingId);
    }
}
