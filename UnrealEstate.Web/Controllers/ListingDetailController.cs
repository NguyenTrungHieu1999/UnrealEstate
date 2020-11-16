using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.Utilities;
using UnrealEstate.ViewModels.Catalog.ListingPhotos;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Controllers.Api;

namespace UnrealEstate.Web.Controllers
{
    public class ListingDetailController : Controller
    {
        private readonly ListingsController _listingsController;

        public ListingDetailController(ListingsController listingsController)
        {
            _listingsController = listingsController;
        }

        public async Task<IActionResult> Index(int id)
        {
            var user = HttpContext.Session.GetComplexData<UserVm>("User");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            var listing = await _listingsController.GetListingById(id);

            ViewBag.ListingPhotos = await _listingsController.GetListingPhotoByListingId(id);

            ViewBag.Comments = await _listingsController.GetAllComment(id);

            ViewBag.Bids = await _listingsController.GetAllBidByListingId(id);

            if(user is null)
            {
                ViewBag.Favorite = false;
            }
            else
            {
                ViewBag.Favorite = await _listingsController.FindFavorite(user.Id, id);
            }

            return View(listing);
        }

        public async Task<IActionResult> DeletePhoTo(int listingId, int photoId)
        {

            if (listingId == 0 || photoId == 0)
            {
                return RedirectToAction("Index", "Listing");
            }

            var result = await _listingsController.DeletePhotoById(photoId);

            if (result == 1)
            {
                TempData["DeletePhotoStatus"] = "Success";
            }

            return Redirect("/ListingDetail/Index/" + listingId.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> CreatePhoto(ListingPhotoRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            var result = await _listingsController.CreatePhoto(request);

            if (result == 1)
            {
                TempData["CreatePhotoStatus"] = "Success";
            }

            return Redirect("/ListingDetail/Index/" + request.ListingId.ToString());
        }

        public async Task<IActionResult> CreateOrRemoveFavorite(int id)
        {
            var user = HttpContext.Session.GetComplexData<UserVm>("User");

            var result = await _listingsController.CreateOrRemoveFavorite(id, user.Id);

            return Redirect("/ListingDetail/Index/" + id.ToString());
        }
    }
}
