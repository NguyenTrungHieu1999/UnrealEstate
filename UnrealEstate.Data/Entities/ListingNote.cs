namespace UnrealEstate.Data.Entities
{
    public class ListingNote
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public int UserId { get; set; }

        public string Text { get; set; }

        public User User { get; set; }

        public Listing Listing { get; set; }
    }
}
