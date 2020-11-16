using System.Collections.Generic;
using UnrealEstate.ViewModels.Common;

namespace UnrealEstate.ViewModels.Catalog.Users
{
    public class RoleAssignRequest
    {
        public int Id { get; set; }

        public List<SelectItem> Roles { get; set; } = new List<SelectItem>();
    }
}
