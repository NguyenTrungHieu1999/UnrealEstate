using System;
using System.Collections.Generic;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.Data.Entities
{
    public class Listing
    {
        public int Id { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string City { get; set; }

        public int Zip { get; set; }

        public string State { get; set; }

        public Status Status { get; set; }

        public int Beds { get; set; }

        public double Size { get; set; }

        public int BuiltYear { get; set; }

        public decimal StartingPrice { get; set; }

        public DateTime DueDate { get; set; }

        public string Description { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string CreateBy { get; set; }

        public string ModifiedBy { get; set; }

        public List<ListingPhoto> ListingPhotos { get; set; }

        public List<Bid> Bids { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Favorite> Favorites { get; set; }

        public List<ListingNote> ListingNotes { get; set; }
    }
}
