using AutoMapper;
using System.Linq;
using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Bids;
using UnrealEstate.ViewModels.Catalog.Comments;
using UnrealEstate.ViewModels.Catalog.Listings;
using UnrealEstate.ViewModels.Catalog.Users;

namespace UnrealEstate.Mapper
{
    public class UnrealEstateProfile : Profile
    {
        UnrealEstateDbContext db = new UnrealEstateDbContext();
        public UnrealEstateProfile()
        {
            CreateMap<Listing, ListingViewModel>();

            CreateMap<ListingCreateRequest, Listing>()
                .ForMember(d => d.Id, s => s.Ignore());

            CreateMap<ListingViewModel, ListingUpdateRequest>();

            CreateMap<Bid, BidViewModel>()
                 .ForMember(d => d.UserName, opt => opt.MapFrom(src => db.Users.Find(src.UserId).UserName));

            CreateMap<BidCreateRequest, Bid>();

            CreateMap<CommentCreateRequest, Comment>();

            CreateMap<Comment, CommentViewRequest>()
                .ForMember(d => d.UserName, opt => opt.MapFrom(src => db.Users.Find(src.UserId).UserName));

            CreateMap<CommentUpdateRequest, Comment>()
                .ForMember(d=>d.Id, s=>s.Ignore());

            CreateMap<ListingPhoto, ListingPhotoViewModel>()
                .ForMember(d=>d.PhotoUrl, opt => opt.MapFrom(src => "https://localhost:5001/user-content/" + src.PhotoUrl));

            CreateMap<UserRegisterRequest, User>();

            CreateMap<UserVm, UserUpdateRequest>();

            CreateMap<User, UserVm>();
        }
    }
}
