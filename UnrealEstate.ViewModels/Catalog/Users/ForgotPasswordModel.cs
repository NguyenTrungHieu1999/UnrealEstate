using System.ComponentModel.DataAnnotations;

namespace UnrealEstate.ViewModels.Catalog.Users
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
