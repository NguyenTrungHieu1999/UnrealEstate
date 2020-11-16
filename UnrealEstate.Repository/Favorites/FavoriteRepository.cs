using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.Favorites
{
    public class FavoriteRepository : RepositoryBase<Favorite>, IFavoriteRepository
    {
        public FavoriteRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
