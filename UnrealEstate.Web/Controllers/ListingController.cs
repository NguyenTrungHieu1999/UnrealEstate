using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.Utilities;
using UnrealEstate.ViewModels.Catalog.Listings;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Controllers.Api;
using UnrealEstate.Web.Services;

namespace UnrealEstate.Web.Controllers
{
    public class ListingController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ListingsController _listingsController;

        public ListingController(IMapper mapper, ListingsController listingsController)
        {
            _mapper = mapper;
            _listingsController = listingsController;
        }

        [HttpGet]
        public async Task<IActionResult> Index(GetListingPagingRequest request)
        {
            var data = await _listingsController.GetAllListing(request);

            if(!data.IsSuccessed)
            {
                ModelState.AddModelError(string.Empty, data.Message);
            }

            return View(data.ResultObj);
        }

        [HttpGet]
        [Authorize(Roles = "User")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Create([FromForm]ListingCreateRequest request)
        {
            var user = HttpContext.Session.GetComplexData<UserVm>("User");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            request.CreateBy = user.UserName;
            request.ModifiedBy = user.UserName;

            var result = await _listingsController.CreateListing(request);

            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            var result = await _listingsController.GetListingById(id);

            var listing = _mapper.Map<ListingUpdateRequest>(result);

            return View(listing);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(ListingUpdateRequest request)
        {
            var user = HttpContext.Session.GetComplexData<UserVm>("User");

            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index", "Listing");
            }

            request.ModifiedBy = user.UserName;

            var result = await _listingsController.UpdateListing(request);

            if (result == 0)
            {
                ModelState.AddModelError("", "Update failed");
                return View();
            }
            return RedirectToAction("Index");
        }
    }
}
