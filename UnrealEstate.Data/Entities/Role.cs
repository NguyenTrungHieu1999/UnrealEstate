using Microsoft.AspNetCore.Identity;

namespace UnrealEstate.Data.Entities
{
    public class Role : IdentityRole<int>
    {
        public string Description { get; set; }
    }
}
