using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.Utilities;
using UnrealEstate.ViewModels.Catalog.Comments;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Controllers.Api;

namespace UnrealEstate.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly CommentsController _commentsController;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentController(IHttpContextAccessor httpContextAccessor, CommentsController commentsController)
        {
            _httpContextAccessor = httpContextAccessor;
            _commentsController = commentsController;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CommentCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            var user = _httpContextAccessor.HttpContext.Session.GetComplexData<UserVm>("User");

            if (user is null)
            {
                return RedirectToAction("Index", "Login");
            }

            request.UserId = user.Id;

            await _commentsController.CreateComment(request);

            return Redirect("/ListingDetail/Index/" + request.ListingId.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CommentUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            var user = _httpContextAccessor.HttpContext.Session.GetComplexData<UserVm>("User");

            if (user is null)
            {
                return RedirectToAction("Index", "Login");
            }

            await _commentsController.UpdateComment(user.Id, request);

            return Redirect("/ListingDetail/Index/" + request.ListingId.ToString());
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int commentId, int listingId)
        {
            if (commentId == 0 && listingId == 0)
            {
                return RedirectToAction("Index", "Listing");
            }

            await _commentsController.RemoveComment(commentId);

            return Redirect("/ListingDetail/Index/" + listingId.ToString());
        }

    }
}
