using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.ViewModels.Catalog.Bids
{
    public class BidCreateRequest
    {
        public int UserId { get; set; }

        public int ListingId { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required]
        public string Comment { get; set; }
    }
}
