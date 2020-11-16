using System.ComponentModel;

namespace UnrealEstate.ViewModels.FilterModel
{
    public class FilterUserModel
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Limit { get; set; }

        public int? Offset { get; set; }
    }
}
