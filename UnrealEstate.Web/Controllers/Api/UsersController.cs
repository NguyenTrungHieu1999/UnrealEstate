using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.Utilities.ActionFilter;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Services.Users;

namespace UnrealEstate.Web.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            var result = await _userService.RegisterAsync(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPut("update")]
        [Authorize]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> Update([FromBody] UserUpdateRequest request)
        {
            var result = await _userService.UpdateUserAsync(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet("me")]
        [Authorize]
        public async Task<UserVm> GetCurrentUser([FromQuery] int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (!user.IsSuccessed)
            {
                return null;
            }

            return user.ResultObj;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (!user.IsSuccessed)
            {
                return BadRequest(user);
            }

            return Ok(user);
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.DeleteUserAsync(id);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUserPaging([FromQuery] GetUserPagingRequest request)
        {
            var result = await _userService.GetUsersPaging(request);

            if (!result.IsSuccessed)
            {
                return BadRequest(result);
            }

            return Ok(result);
        }
    }
}
