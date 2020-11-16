using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Comments;

namespace UnrealEstate.Web.Services.Comments
{
    public interface ICommentServices
    {
        Task<int> CreateCommentAsync(CommentCreateRequest request);

        Task<int> UpdateCommentAsync(CommentUpdateRequest request);

        Task<int> RemoveCommentAsync(int Id);

        Task<List<Comment>> GetAllCommentByListingId(int listingId);
    }
}
