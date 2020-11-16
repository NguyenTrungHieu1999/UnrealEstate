using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.Comments
{
    public class CommentRepository : RepositoryBase<Comment>, ICommmentRepository
    {
        public CommentRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
