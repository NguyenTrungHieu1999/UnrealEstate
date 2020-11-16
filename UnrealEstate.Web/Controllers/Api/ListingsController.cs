using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Utilities.ActionFilter;
using UnrealEstate.ViewModels.Catalog.Bids;
using UnrealEstate.ViewModels.Catalog.Comments;
using UnrealEstate.ViewModels.Catalog.ListingPhotos;
using UnrealEstate.ViewModels.Catalog.Listings;
using UnrealEstate.ViewModels.Common;
using UnrealEstate.Web.Services.Bids;
using UnrealEstate.Web.Services.Listings;

namespace UnrealEstate.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListingsController : ControllerBase
    {
        private readonly IListingService _listingService;
        private readonly IBidService _bidService;

        public ListingsController(IListingService listingService, IBidService bidService)
        {
            _listingService = listingService;
            _bidService = bidService;
        }

        [HttpGet]
        public async Task<ApiResult<PagedResult<ListingViewModel>>> GetAllListing([FromQuery] GetListingPagingRequest filter)
        {
            var listings = await _listingService.GetListingPaging(filter);

            return listings;
        }

        [HttpGet("{id}")]
        public async Task<ListingViewModel> GetListingById(int id)
        {
            var listing = await _listingService.GetListingByIdAsync(id);

            return listing;
        }

        [HttpPost]
        [Authorize(Roles = "User")]
        [Consumes("multipart/form-data")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<int> CreateListing([FromForm] ListingCreateRequest request)
        {

            var result = await _listingService.CreateListingAsync(request);

            return result;
        }

        [HttpPut("{id}")]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<int> UpdateListing([FromBody] ListingUpdateRequest request)
        {
            var result = await _listingService.UpdateListingAsync(request);

            return result;
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<bool> DisableListing(int id)
        {
            var result = await _listingService.DisableListingAsync(id);

            return result;
        }

        [HttpPost("{id}/enable")]
        [Authorize(Roles = "Admin")]
        public async Task<bool> EnableListing(int id)
        {
            var result = await _listingService.EnableListingAsync(id);

            return result;
        }

        [HttpPost("bid")]
        [Authorize(Roles = "User")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<int> CreateBid([FromBody] BidCreateRequest request)
        {
            var result = await _listingService.CreateBidAsync(request);

            return result;
        }

        [HttpPost("{id}/favorite")]
        public async Task<int> CreateOrRemoveFavorite(int id, [FromBody] int userId)
        {
            var result = await _listingService.CreateOrRemoveFavoriteAsync(id, userId);

            return result;
        }

        [HttpGet("userId={userId}&listingId={listingId}/favorite")]
        public async Task<bool> FindFavorite(int userId, int listingId)
        {
            var result = await _listingService.FindFavoriteById(userId, listingId);

            return result;
        }


        [HttpGet("{id}/comments")]
        public async Task<List<CommentViewRequest>> GetAllComment(int id)
        {
            var result = await _listingService.GetAllCommentAsync(id);

            return result;
        }

        [HttpGet("{id}/listing-photos")]
        public async Task<List<ListingPhotoViewModel>> GetListingPhotoByListingId(int id)
        {
            var result = await _listingService.GetListListingPhotoAsync(id);

            return result;
        }

        [HttpDelete("{photoId}/delete-photo")]
        public async Task<int> DeletePhotoById(int photoId)
        {
            var result = await _listingService.RemovePhotoAsync(photoId);

            return result;
        }

        [HttpPost("create-photo")]
        [Consumes("multipart/form-data")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<int> CreatePhoto([FromForm] ListingPhotoRequest request)
        {
            var result = await _listingService.AddListingPhotoAsync(request);

            return result;
        }

        [HttpGet("{listingId}/all-bid")]
        [AllowAnonymous]
        public async Task<List<BidViewModel>> GetAllBidByListingId(int listingId)
        {
            var result = await _bidService.GetBidsByListingId(listingId);

            return result;
        }
    }
}