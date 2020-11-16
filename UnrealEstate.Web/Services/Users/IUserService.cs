using System.Collections.Generic;
using System.Threading.Tasks;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.ViewModels.Common;
using UnrealEstate.ViewModels.FilterModel;

namespace UnrealEstate.Web.Services.Users
{
    public interface IUserService
    {
        Task<ApiResult<string>> AuthenticateAsync(LoginRequest request);

        Task<ApiResult<bool>> RegisterAsync(UserRegisterRequest request);

        Task<ApiResult<bool>> UpdateUserAsync(UserUpdateRequest request);

        //Task<List<UserVm>> GetAllUserAsync(FilterUserModel filterUserModel);

        Task<ApiResult<UserVm>> GetUserByIdAsync(int id);

        Task<ApiResult<bool>> DeleteUserAsync(int id);

        Task<ApiResult<PagedResult<UserVm>>> GetUsersPaging(GetUserPagingRequest request);

        //Task<ApiResult<bool>> SendForgotPasswordAsync(string email);

        //Task<ApiResult<bool>> RoleAssign(int id, RoleAssignRequest request);
    }
}
