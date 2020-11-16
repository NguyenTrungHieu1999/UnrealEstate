using UnrealEstate.ViewModels.Common;

namespace UnrealEstate.ViewModels.Catalog.Users
{
    public class GetUserPagingRequest : PagingRequestBase
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }

        public string OrderBy { get; set; }
    }
}
