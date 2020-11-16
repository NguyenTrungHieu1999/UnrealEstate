using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.Users
{
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
