namespace UnrealEstate.ViewModels.Catalog.Listings
{
    public class ListingPhotoViewModel
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public string PhotoUrl { get; set; }

        public bool IsDefault { get; set; }
    }
}
