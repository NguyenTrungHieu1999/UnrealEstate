using System;

namespace UnrealEstate.Data.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public int UserId { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Text { get; set; }

        public User User { get; set; }

        public Listing Listing { get; set; }
    }
}
