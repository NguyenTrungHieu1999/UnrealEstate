namespace UnrealEstate.Data.Entities
{
    public class ListingPhoto
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsDefault { get; set; }

        public Listing Listing { get; set; }
    }
}
