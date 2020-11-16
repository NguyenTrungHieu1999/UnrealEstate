using UnrealEstate.ViewModels.Common;

namespace UnrealEstate.ViewModels.Catalog.Listings
{
    public class GetListingPagingRequest : PagingRequestBase
    {
        public decimal? MaxPrice { get; set; }

        public int? MaxAge { get; set; }

        public double? MinSize { get; set; }

        public bool? Sold { get; set; }

        public string Address { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public string OrderBy { get; set; }
    }
}
