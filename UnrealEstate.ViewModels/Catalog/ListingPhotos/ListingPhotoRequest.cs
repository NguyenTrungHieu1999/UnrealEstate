
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.ViewModels.Catalog.ListingPhotos
{
    public class ListingPhotoRequest
    {
        public int ListingId { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "You must choose a file")]
        public IFormFile PhotoFile { get; set; }
    }
}
