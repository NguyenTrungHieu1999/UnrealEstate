using System;
using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.ViewModels.Catalog.Comments
{
    public class CommentUpdateRequest
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
