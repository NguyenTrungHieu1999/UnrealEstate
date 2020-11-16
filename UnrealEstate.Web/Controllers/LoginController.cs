using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.Data.Enums;
using UnrealEstate.Utilities;
using UnrealEstate.Utilities.Constants;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.ViewModels.Common;
using UnrealEstate.Web.Controllers.Api;
using UnrealEstate.Web.Models;

namespace UnrealEstate.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly AuthController _authController;
        private readonly UsersController _usersController;

        public LoginController(IHttpContextAccessor httpContextAccessor, SignInManager<User> signInManager,
            UserManager<User> userManager, AuthController authController, UsersController usersController, 
            IConfiguration configuration)
        {
            _httpContextAccessor = httpContextAccessor;
            _signInManager = signInManager;
            _userManager = userManager;
            _authController = authController;
            _usersController = usersController;
            _configuration = configuration;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string returnUrl)
        {
            await _signInManager.SignOutAsync();

            LoginRequest loginRequest = new LoginRequest
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            return View(loginRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            var token = (ObjectResult)_authController.Authenticate(request).Result;

            ApiResult<string> result = (ApiResult<string>)token.Value;

            if (result.ResultObj is null)
            {
                ModelState.AddModelError(string.Empty, result.Message);

                request.ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                return View(request);
            }

            _httpContextAccessor.HttpContext.Session.SetString(SystemConstants.AppSettings.Token, result.ResultObj);

            var decodeToken = DecodeToken.Decode(_httpContextAccessor.HttpContext.Session.GetString("Token"));

            var userId = decodeToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

            var user = _usersController.GetCurrentUser(int.Parse(userId)).Result;

            _httpContextAccessor.HttpContext.Session.SetComplexData("User", user);

            return RedirectToAction("Index", "Listing");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginsCallback", "Login", new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);

            return new ChallengeResult(provider, properties);
        }


        public async Task<IActionResult> ExternalLoginsCallback(string returnUrl = null, string remoteError = null)
        {
            returnUrl = returnUrl ?? Url.Content("~/");

            LoginRequest loginRequest = new LoginRequest
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Error from external provider:{remoteError}");

                return View("Index", loginRequest);
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();

            if (info is null)
            {
                ModelState.AddModelError(string.Empty, "Error loading external login information");

                return View("Index", loginRequest);
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user is null)
                    {
                        user = new User
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Name).Replace(" ", string.Empty),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email),
                            PhoneNumber = "01235587",
                            FirstName = info.Principal.FindFirstValue(ClaimTypes.GivenName),
                            LastName = info.Principal.FindFirstValue(ClaimTypes.Surname),
                            Birthday = DateTime.Now,
                            Gender = Gender.Male,
                            Status = UserStatus.Active,
                            PasswordHash = "Hieu@123",
                            SecurityStamp = Guid.NewGuid().ToString("D")
                        };

                        var test = await _userManager.CreateAsync(user, user.PasswordHash);
                    }

                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return LocalRedirect(returnUrl);
                }
            }

            ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";
            ViewBag.ErrorMessage = "Please contact support on hieutanmy321@gmail.com";

            return View("Error");
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(UserRegisterRequest request)
        {
            bool isCapthcaValid = ValidateCaptcha(Request.Form["g-recaptcha-response"]);

            if (!ModelState.IsValid)
                return View();

            if (isCapthcaValid)
            {
                //some code After success
                var result = (ObjectResult)_usersController.Register(request).Result;

                var test = (ApiResult<bool>)result.Value;

                if (test.IsSuccessed)
                {
                    return RedirectToAction("Index");
                }

                ModelState.AddModelError(string.Empty, test.Message);
                return View(request);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "You have put wrong Captcha, Please ensure the authenticity !!!");
                //ModelState.Remove("Password");
                //Should load sitekey again
                return View();
            }
        }

        [AllowAnonymous]
        public bool ValidateCaptcha(string response)
        {
            //  Setting _Setting = repositorySetting.GetSetting;

            //secret that was generated in key value pair  
            string secret = _configuration["reCAPTCHA:SecretKey"];

            var client = new WebClient();
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, response));

            var captchaResponse = JsonConvert.DeserializeObject<CaptchaResponse>(reply);

            return Convert.ToBoolean(captchaResponse.Success);

        }
    }
}
