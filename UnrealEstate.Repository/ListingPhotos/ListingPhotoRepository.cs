using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.ListingPhotos
{
    public class ListingPhotoRepository : RepositoryBase<ListingPhoto>, IListingPhotoRepository
    {
        public ListingPhotoRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
