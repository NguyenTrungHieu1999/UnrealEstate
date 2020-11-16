using System;

namespace UnrealEstate.ViewModels.Catalog.Comments
{
    public class CommentViewRequest
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime ModifiedDate { get; set; }

        public string Text { get; set; }
    }
}
