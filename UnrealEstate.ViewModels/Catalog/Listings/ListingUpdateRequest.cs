using System;
using System.ComponentModel.DataAnnotations;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.ViewModels.Catalog.Listings
{
    public class ListingUpdateRequest
    {
        public int Id { get; set; }
        [Required]
        public string AddressLine1 { get; set; }

        [Required]
        public string AddressLine2 { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public int Zip { get; set; }

        [Required]
        public string State { get; set; }

        [Required]
        public Status Status { get; set; }

        [Required]
        public int Beds { get; set; }

        [Required]
        public double Size { get; set; }

        [Required]
        public int BuiltYear { get; set; }

        public string ModifiedBy { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal StartingPrice { get; set; }

        [DataType(DataType.Date)]
        public DateTime DueDate { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
