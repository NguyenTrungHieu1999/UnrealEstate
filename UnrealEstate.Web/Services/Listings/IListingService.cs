using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Bids;
using UnrealEstate.ViewModels.Catalog.Comments;
using UnrealEstate.ViewModels.Catalog.ListingPhotos;
using UnrealEstate.ViewModels.Catalog.Listings;
using UnrealEstate.ViewModels.Common;

namespace UnrealEstate.Web.Services.Listings
{
    public interface IListingService
    {
        #region Listing
        Task<int> CreateListingAsync(ListingCreateRequest request);

        Task<int> UpdateListingAsync(ListingUpdateRequest request);

        Task<int> DeleteListingAsync(int listingId);

        Task<ListingViewModel> GetListingByIdAsync(int listingId);

        Task<ApiResult<PagedResult<ListingViewModel>>> GetListingPaging(GetListingPagingRequest request);

        Task<Listing> FindListingByIdAsync(int listingId);

        Task<List<ListingViewModel>> GetAllListingAsync(GetListingPagingRequest filter);

        Task<bool> DisableListingAsync(int listingId);

        Task<bool> EnableListingAsync(int listingId);
        #endregion Listing

        #region ListingPhoto
        Task<int> AddListingPhotoAsync(ListingPhotoRequest request);

        Task<ListingPhotoViewModel> GetListingPhotoByIdAsync(int photoId);

        Task<List<ListingPhotoViewModel>> GetListListingPhotoAsync(int listingId);

        Task<int> RemovePhotoAsync(int photoId);

        Task<int> UpdateListingPhotoAsync(int photoId, IFormFile photoUrl);
        #endregion ListingPhoto

        Task<int> CreateBidAsync(BidCreateRequest request);

        Task<int> CreateOrRemoveFavoriteAsync(int listingId, int userId);

        Task<bool> FindFavoriteById(int userId, int listingId);

        Task<List<CommentViewRequest>> GetAllCommentAsync(int listingId);
    }
}
