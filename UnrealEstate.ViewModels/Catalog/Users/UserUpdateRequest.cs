using System;
using System.ComponentModel.DataAnnotations;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.ViewModels.Catalog.Users
{
    public class UserUpdateRequest
    {
        public int Id { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        public string FirstName { get; set; }

        [Required, StringLength(20, MinimumLength = 1)]
        public string LastName { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required, DataType(DataType.PhoneNumber), StringLength(11)]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public UserStatus Status { get; set; }
    }
}
