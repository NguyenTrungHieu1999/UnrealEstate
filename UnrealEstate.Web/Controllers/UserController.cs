using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.Utilities;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.ViewModels.Common;
using UnrealEstate.Web.Controllers.Api;

namespace UnrealEstate.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly UsersController _usersController;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;

        public UserController(IMapper mapper, IHttpContextAccessor httpContextAccessor, 
            SignInManager<User> signInManager, UsersController usersController)
        {
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _usersController = usersController;
        }

        public IActionResult Index(GetUserPagingRequest request)
        {
            var data = (ObjectResult)_usersController.GetAllUserPaging(request).Result;

            var users = (ApiResult<PagedResult<UserVm>>)data.Value;

            if (users is null)
            {
                return View();
            }

            ViewBag.Role = users.Message;

            return View(users.ResultObj);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {

            await _signInManager.SignOutAsync();

            _httpContextAccessor.HttpContext.Session.Remove("Token");

            return RedirectToAction("Index", "Login");
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(UserRegisterRequest request)
        {
            if (!ModelState.IsValid)
                return View();

            var result = (ObjectResult)_usersController.Register(request).Result;

            var status = (ApiResult<bool>)result.Value;

            if (status.IsSuccessed)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, status.Message);

            return View(request);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(int id)
        {
            if(id == 0)
            {
                return RedirectToAction("Index", "Listing");
            }

            var result = (ObjectResult)_usersController.GetUserById(id).Result;

            var user = (ApiResult<UserVm>)result.Value;

            var model = _mapper.Map<UserUpdateRequest>(user.ResultObj);

            ViewBag.UserId = id.ToString();

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(UserUpdateRequest request)
        {

            var result = (ObjectResult)_usersController.Update(request).Result;

            var status = (ApiResult<bool>)result.Value;

            if (status.IsSuccessed)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError(string.Empty, status.Message);

            return View();
        }

        [HttpGet]
        public IActionResult Profile()
        {
            var userId = HttpContext.Session.GetComplexData<UserVm>("User").Id;

            var user = _usersController.GetCurrentUser(userId).Result;

            return View(user);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                return RedirectToAction("Index", "Listing");
            }

            await _usersController.Delete(id);

            return RedirectToAction("Index");
        }
    }
}
