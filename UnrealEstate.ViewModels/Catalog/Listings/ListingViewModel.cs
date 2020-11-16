using System;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.ViewModels.Catalog.Listings
{
    public class ListingViewModel
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public int Zip { get; set; }

        public string State { get; set; }

        public Status Status { get; set; }

        public int Beds { get; set; }

        public string Size { get; set; }

        public int BuiltYear { get; set; }

        public decimal StartingPrice { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreateBy { get; set; }

        public string ModifiedBy { get; set; }

        public decimal MaxPrice { get; set; }

        public string PhotoUrl { get; set; }
    }
}
