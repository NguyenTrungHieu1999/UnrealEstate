using System.Collections.Generic;
using UnrealEstate.ViewModels.Catalog.Bids;

namespace UnrealEstate.ViewModels.Catalog.Listings
{
    public class ListingBidViewModel
    {
        public ListingViewModel ListingViewModel { get; set; }

        public List<BidViewModel> BidViewModels { get; set; }
    }
}
