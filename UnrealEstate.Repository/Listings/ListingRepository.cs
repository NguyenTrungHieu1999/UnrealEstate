using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.Listings
{
    public class ListingRepository : RepositoryBase<Listing>, IListingRepository
    {
        public ListingRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
