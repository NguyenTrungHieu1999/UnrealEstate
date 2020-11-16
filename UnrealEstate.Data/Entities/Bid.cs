using System;

namespace UnrealEstate.Data.Entities
{
    public class Bid
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public int UserId { get; set; }

        public decimal Price { get; set; }

        public DateTime CreateDate { get; set; }

        public string Comment { get; set; }

        public User User { get; set; }

        public Listing Listing { get; set; }
    }
}
