using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.ViewModels.Catalog.Comments;
using UnrealEstate.Web.Services.Comments;

namespace UnrealEstate.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentServices _comment;

        public CommentsController(ICommentServices comment)
        {
            _comment = comment;
        }

        [HttpPost]
        [Authorize]
        public async Task<int> CreateComment([FromBody] CommentCreateRequest request)
        {
            var result = await _comment.CreateCommentAsync(request);

            return result;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<int> UpdateComment(int id, [FromBody] CommentUpdateRequest request)
        {
            var result = await _comment.UpdateCommentAsync(request);

            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<int> RemoveComment(int id)
        {
            var result = await _comment.RemoveCommentAsync(id);

            return result;
        }

        [HttpGet("{listingId}")]
        public async Task<IActionResult> GetAllComment(int listingId)
        {
            var comments = await _comment.GetAllCommentByListingId(listingId);

            return Ok(comments);
        }
    }
}
