using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.Data.Enums;
using UnrealEstate.Repository.Wrapper;
using UnrealEstate.Utilities.Exceptions;
using UnrealEstate.ViewModels.Catalog.Bids;
using UnrealEstate.ViewModels.Catalog.Comments;
using UnrealEstate.ViewModels.Catalog.ListingPhotos;
using UnrealEstate.ViewModels.Catalog.Listings;
using UnrealEstate.ViewModels.Common;

namespace UnrealEstate.Web.Services.Listings
{
    public class ListingService : IListingService
    {
        private readonly IMapper _mapper;
        private readonly IRepositoryWrapper _repository;
        private readonly Common.IStorageService _storageService;
        private readonly UserManager<User> _userManager;

        public ListingService(IMapper mapper, IRepositoryWrapper repository,
            Common.IStorageService storageService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _repository = repository;
            _storageService = storageService;
            _userManager = userManager;
        }

        #region Listing

        public async Task<int> CreateListingAsync(ListingCreateRequest request)
        {
            User user = await _repository.UserRepo.FindByCondition(x => x.UserName == request.CreateBy)
                                                    .SingleOrDefaultAsync();

            Listing listing = _mapper.Map<Listing>(request);

            listing.CreateDate = DateTime.Now;
            listing.ModifiedDate = DateTime.Now;

            listing.ListingNotes = new List<ListingNote>()
            {
                new ListingNote()
                {
                    UserId = user.Id,
                    Text = request.ListingNote
                }
            };

            if (request.ThumbnailImage != null)
            {
                listing.ListingPhotos = new List<ListingPhoto>()
                {
                    new ListingPhoto()
                    {
                        PhotoUrl = await SaveFile(request.ThumbnailImage),
                        IsDefault = true
                    }
                };
            }

            await _repository.ListingRepo.CreateAsync(listing);
            await _repository.SaveChangesAsync();

            return listing.Id;
        }

        public async Task<int> DeleteListingAsync(int listingId)
        {
            Listing listing = await FindListingByIdAsync(listingId);

            await RemoveAllListingPhotoAsync(listingId);

            _repository.ListingRepo.Delete(listing);

            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> DisableListingAsync(int listingId)
        {
            Listing listing = await FindListingByIdAsync(listingId);

            listing.Status = Status.Disabled;

            _repository.ListingRepo.Update(listing);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> EnableListingAsync(int listingId)
        {
            Listing listing = await FindListingByIdAsync(listingId);

            listing.Status = Status.Active;

            _repository.ListingRepo.Update(listing);

            await _repository.SaveChangesAsync();

            return true;
        }

        public async Task<Listing> FindListingByIdAsync(int listingId)
        {
            Listing listing = await _repository.ListingRepo.FindByIdAsync(listingId);

            if (listing is null)
            {
                throw new UnrealEstateException($"Can not find listing with id: {listingId}");
            }

            return listing;
        }

        public async Task<List<ListingViewModel>> GetAllListingAsync(GetListingPagingRequest filter)
        {
            List<Listing> query = await _repository.ListingRepo.FindByCondition(x => x.Status != Status.Disabled).ToListAsync();

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(x => x.StartingPrice < filter.MaxPrice).ToList();
            }

            if (filter.MaxAge.HasValue)
            {
                query = query.Where(x => x.BuiltYear < filter.MaxAge).ToList();
            }

            if (filter.MinSize.HasValue)
            {
                query = query.Where(x => x.Size > filter.MinSize).ToList();
            }

            if (filter.Limit.HasValue)
            {
                query = query.OrderByDescending(x => x.Id)
                                .Skip(filter.Limit.Value * (filter.Offset.Value - 1))
                                .Take(filter.Limit.Value).ToList();
            }

            if (filter.Sold.HasValue)
            {
                query = query.Where(x => x.Status == Status.Sold).ToList();
            }

            if (filter.Address != null)
            {
                query = query.Where(x => x.AddressLine1.Contains(filter.Address)
                || x.Zip.ToString().Contains(filter.Address)
                || x.City.Contains(filter.Address)
                || x.State.Contains(filter.Address)).ToList();
            }

            if (filter.OrderBy != null)
            {
                query = query.OrderBy(x => x.GetType().GetProperty(filter.OrderBy).GetValue(x, null)).ToList();
            }

            List<ListingViewModel> data = _mapper.Map<List<Listing>, List<ListingViewModel>>(query);

            return data;
        }

        public async Task<ApiResult<PagedResult<ListingViewModel>>> GetListingPaging(GetListingPagingRequest request)
        {
            var query = await GetAllListingAsync(request);

            foreach (var item in query)
            {
                var listingPhotos = await GetListListingPhotoAsync(item.Id);

                if (listingPhotos.Count > 0)
                {
                    var photos = listingPhotos.Where(x => x.IsDefault == true).FirstOrDefault();

                    if (photos != null)
                    {
                        item.PhotoUrl = photos.PhotoUrl;
                    }
                }
            }

            int totalRow = query.Count();

            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => x)
                .ToList();

            var pageResult = new PagedResult<ListingViewModel>()
            {
                TotalRecords = totalRow,
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                Items = data
            };

            if (data is null)
            {
                return new ApiErrorResult<PagedResult<ListingViewModel>>("No records found");
            }

            return new ApiSuccessResult<PagedResult<ListingViewModel>>(pageResult);
        }

        public async Task<ListingViewModel> GetListingByIdAsync(int listingId)
        {
            Listing query = await FindListingByIdAsync(listingId);

            var bids = await _repository.BidRepo.FindByCondition(x => x.ListingId == listingId).ToListAsync();

            decimal maxPrice = query.StartingPrice;

            if (bids.Count > 0)
            {
                maxPrice = bids.Max(x => x.Price);
            }

            var result = _mapper.Map<ListingViewModel>(query);

            result.MaxPrice = maxPrice;

            return result;
        }

        public async Task<int> UpdateListingAsync(ListingUpdateRequest request)
        {
            User user = await _userManager.FindByNameAsync(request.ModifiedBy);

            Listing listing = await FindListingByIdAsync(request.Id);

            if (user.UserName == listing.CreateBy || await _userManager.IsInRoleAsync(user, "Admin"))
            {
                listing.Map(request);

                _repository.ListingRepo.Update(listing);

                return await _repository.SaveChangesAsync();
            }

            return 0;
        }
        #endregion ListingPhoto

        #region ListingPhoto
        public async Task<int> AddListingPhotoAsync(ListingPhotoRequest request)
        {
            int count = _repository.ListingPhotoRepo.FindByCondition(x => x.ListingId == request.ListingId).ToList().Count;

            if (count >= 5)
            {
                return 0;
            }

            var listingPhoto = new ListingPhoto()
            {
                ListingId = request.ListingId,
                IsDefault = request.IsDefault
            };

            if (request.PhotoFile != null)
            {
                listingPhoto.PhotoUrl = await SaveFile(request.PhotoFile);
            }

            await _repository.ListingPhotoRepo.CreateAsync(listingPhoto);
            await _repository.SaveChangesAsync();

            return listingPhoto.Id;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');

            var fileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";

            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);

            return fileName;
        }

        public async Task<ListingPhotoViewModel> GetListingPhotoByIdAsync(int photoId)
        {
            ListingPhoto listingPhoto = await FindListingPhotoByIdAsync(photoId);

            ListingPhotoViewModel viewModel = _mapper.Map<ListingPhotoViewModel>(listingPhoto);

            return viewModel;
        }



        public async Task<List<ListingPhotoViewModel>> GetListListingPhotoAsync(int listingId)
        {
            List<ListingPhoto> listingPhotos = await _repository.ListingPhotoRepo
                                                                .FindByCondition(x => x.ListingId == listingId)
                                                                .ToListAsync();

            return _mapper.Map<List<ListingPhoto>, List<ListingPhotoViewModel>>(listingPhotos);
        }

        public async Task<int> RemovePhotoAsync(int photoId)
        {
            ListingPhoto listingPhoto = await FindListingPhotoByIdAsync(photoId);

            _repository.ListingPhotoRepo.Delete(listingPhoto);

            await _storageService.DeleteFileAsync(listingPhoto.PhotoUrl);

            return await _repository.SaveChangesAsync();
        }

        private async Task<int> RemoveAllListingPhotoAsync(int listingId)
        {
            List<ListingPhoto> listingPhotos = await _repository.ListingPhotoRepo
                                                                .FindByCondition(x => x.ListingId == listingId)
                                                                .ToListAsync();

            foreach (var photo in listingPhotos)
            {
                await _storageService.DeleteFileAsync(photo.PhotoUrl);
            }

            return await _repository.SaveChangesAsync();
        }

        public async Task<int> UpdateListingPhotoAsync(int photoId, IFormFile photoUrl)
        {
            ListingPhoto listingPhoto = await FindListingPhotoByIdAsync(photoId);

            if (photoUrl != null)
            {
                listingPhoto.PhotoUrl = await SaveFile(photoUrl);
            }

            _repository.ListingPhotoRepo.Update(listingPhoto);

            return await _repository.SaveChangesAsync();
        }

        private async Task<ListingPhoto> FindListingPhotoByIdAsync(int photoId)
        {
            ListingPhoto listingPhoto = await _repository.ListingPhotoRepo.FindByIdAsync(photoId);

            return listingPhoto;
        }
        #endregion ListingPhoto

        #region Bid
        public async Task<int> CreateBidAsync(BidCreateRequest request)
        {
            Bid bid = _mapper.Map<Bid>(request);

            bid.CreateDate = DateTime.Now;
            bid.ListingId = request.ListingId;

            await _repository.BidRepo.CreateAsync(bid);

            return await _repository.SaveChangesAsync();
        }
        #endregion Bid

        #region Favorite
        public async Task<int> CreateOrRemoveFavoriteAsync(int listingId, int userId)
        {
            Favorite favorite = await _repository.FavoriteRepo
                                                    .FindByCondition(x => x.ListingId == listingId && x.UserId == userId)
                                                    .SingleOrDefaultAsync();

            if (favorite is null)
            {
                favorite = new Favorite();
                favorite.ListingId = listingId;
                favorite.UserId = userId;

                await _repository.FavoriteRepo.CreateAsync(favorite);
            }
            else
            {
                _repository.FavoriteRepo.Delete(favorite);
            }

            return await _repository.SaveChangesAsync();
        }

        public async Task<bool> FindFavoriteById(int userId, int listingId)
        {
            var favorite = await _repository.FavoriteRepo.FindByCondition(x => x.ListingId == listingId && x.UserId == userId).SingleOrDefaultAsync();

            if (favorite is null)
            {
                return false;
            }

            return true;
        }
        #endregion Favorite

        #region Comment
        public async Task<List<CommentViewRequest>> GetAllCommentAsync(int listingId)
        {
            List<Comment> comments = await _repository.CommmentRepo
                                                        .FindByCondition(x => x.ListingId == listingId)
                                                        .ToListAsync();

            List<CommentViewRequest> result = _mapper.Map<List<Comment>, List<CommentViewRequest>>(comments);

            return result;
        }
        #endregion Comment
    }
}