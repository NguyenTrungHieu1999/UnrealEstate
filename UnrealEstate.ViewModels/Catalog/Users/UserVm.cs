using System;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.ViewModels.Catalog.Users
{
    public class UserVm
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Birthday { get; set; }

        public Gender Gender { get; set; }

        public string PhoneNumber { get; set; }

        public UserStatus Status { get; set; }
    }
}
