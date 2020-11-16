using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.Data.Entities
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; } // 0.Male or 1.female

        public UserStatus Status { get; set; }

        public List<Bid> Bids { get; set; }

        public List<Comment> Comments { get; set; }

        public List<Favorite> Favorites { get; set; }

        public List<ListingNote> ListingNotes { get; set; }
    }
}
