using System;

namespace UnrealEstate.ViewModels.Catalog.Bids
{
    public class BidViewModel
    {
        public string UserName { get; set; }

        public decimal Price { get; set; }

        public DateTime CreateDate { get; set; }

        public string Comment { get; set; }
    }
}
