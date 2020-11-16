using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.ViewModels.Catalog.Comments
{
    public class CommentCreateRequest
    {
        public int ListingId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
