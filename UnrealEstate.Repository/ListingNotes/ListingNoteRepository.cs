using UnrealEstate.Data.EF;
using UnrealEstate.Data.Entities;

namespace UnrealEstate.Repository.ListingNotes
{
    public class ListingNoteRepository : RepositoryBase<ListingNote>, IListingNoteRepository
    {
        public ListingNoteRepository(UnrealEstateDbContext context) : base(context)
        {
        }
    }
}
