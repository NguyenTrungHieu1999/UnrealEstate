using System.Threading.Tasks;
using UnrealEstate.Data.EF;
using UnrealEstate.Repository.Bids;
using UnrealEstate.Repository.Comments;
using UnrealEstate.Repository.Favorites;
using UnrealEstate.Repository.ListingNotes;
using UnrealEstate.Repository.ListingPhotos;
using UnrealEstate.Repository.Listings;
using UnrealEstate.Repository.Users;

namespace UnrealEstate.Repository.Wrapper
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private UnrealEstateDbContext _repoContext;

        private IListingRepository _listing;
        private IBidRepository _bid;
        private IListingPhotoRepository _listingPhoto;
        private IFavoriteRepository _favorite;
        private ICommmentRepository _comment;
        private IUserRepository _user;
        private IListingNoteRepository _listingNote;

        public RepositoryWrapper(UnrealEstateDbContext repoContext)
        {
            _repoContext = repoContext;
        }

        public IListingRepository ListingRepo
        {
            get
            {
                if (_listing == null)
                {
                    _listing = new ListingRepository(_repoContext);
                }
                return _listing;
            }
        }

        public IBidRepository BidRepo
        {
            get
            {
                if (_bid == null)
                {
                    _bid = new BidRepository(_repoContext);
                }

                return _bid;
            }
        }

        public IListingPhotoRepository ListingPhotoRepo
        {
            get
            {
                if (_listingPhoto == null)
                {
                    _listingPhoto = new ListingPhotoRepository(_repoContext);
                }

                return _listingPhoto;
            }
        }

        public IFavoriteRepository FavoriteRepo
        {
            get
            {
                if (_favorite == null)
                {
                    _favorite = new FavoriteRepository(_repoContext);
                }

                return _favorite;
            }
        }

        public ICommmentRepository CommmentRepo
        {
            get
            {
                if(_comment == null)
                {
                    _comment = new CommentRepository(_repoContext);
                }

                return _comment;
            }
        }

        public IUserRepository UserRepo 
        {
            get
            {
                if(_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }

        public IListingNoteRepository NoteRepo
        {
            get
            {
                if (_listingNote == null)
                {
                    _listingNote = new ListingNoteRepository(_repoContext);
                }
                return _listingNote;
            }
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _repoContext.SaveChangesAsync();
        }
    }
}
