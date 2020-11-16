using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.Utilities;
using UnrealEstate.ViewModels.Catalog.Bids;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Controllers.Api;

namespace UnrealEstate.Web.Controllers
{
    public class BidController : Controller
    {
        private readonly ListingsController _listingsController;

        public BidController(ListingsController listingsController)
        {
            _listingsController = listingsController;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Create(BidCreateRequest request)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            var user = HttpContext.Session.GetComplexData<UserVm>("User");

            request.UserId = user.Id;

            int status = await _listingsController.CreateBid(request);

            if (status != 0)
            {
                TempData["CreateBidStatus"] = "Success";
            }

            return Redirect("/ListingDetail/Index/" + request.ListingId.ToString());
        }
    }
}
