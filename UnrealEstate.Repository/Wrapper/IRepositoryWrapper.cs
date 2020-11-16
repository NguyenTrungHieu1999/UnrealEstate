using System.Threading.Tasks;
using UnrealEstate.Repository.Bids;
using UnrealEstate.Repository.Comments;
using UnrealEstate.Repository.Favorites;
using UnrealEstate.Repository.ListingNotes;
using UnrealEstate.Repository.ListingPhotos;
using UnrealEstate.Repository.Listings;
using UnrealEstate.Repository.Users;

namespace UnrealEstate.Repository.Wrapper
{
    public interface IRepositoryWrapper
    {
        IListingRepository ListingRepo { get;}

        IBidRepository BidRepo { get; }

        IListingPhotoRepository ListingPhotoRepo { get; }

        IFavoriteRepository FavoriteRepo { get; }

        ICommmentRepository CommmentRepo { get; }

        IUserRepository UserRepo { get; }

        IListingNoteRepository NoteRepo { get; }

        Task<int> SaveChangesAsync();
    }
}
