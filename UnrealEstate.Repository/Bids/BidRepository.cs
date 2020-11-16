using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.Bids
{
    public class BidRepository : RepositoryBase<Bid>, IBidRepository
    {
        public BidRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
