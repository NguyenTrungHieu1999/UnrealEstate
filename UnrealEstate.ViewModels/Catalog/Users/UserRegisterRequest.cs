using System;
using System.ComponentModel.DataAnnotations;
using UnrealEstate.Data.Enums;

namespace UnrealEstate.ViewModels.Catalog.Users
{
    public class UserRegisterRequest
    {
        [Required, StringLength(20, MinimumLength = 1)]
        public string UserName { get; set; }

        [Required, DataType(DataType.Password)]
        [RegularExpression("(?=^.{8,10}$)(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+}{\":;'?/>.<,])(?!.*\\s).*$", ErrorMessage = "Validation for 8-10 characters with characters,numbers,1 upper case letter and special characters.")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }

        [Required, DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }

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
    }
}
