using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Users;

namespace UnrealEstate.Web.Services.Users
{
    public static class UserExtensions
    {
        public static void Map(this User user, UserUpdateRequest request)
        {
            user.FirstName = request.FirstName;
            user.LastName = request.LastName;
            user.PhoneNumber = request.PhoneNumber;
            user.Gender = request.Gender;
            user.Birthday = request.Birthday;
            user.Status = request.Status;
        }
    }
}
