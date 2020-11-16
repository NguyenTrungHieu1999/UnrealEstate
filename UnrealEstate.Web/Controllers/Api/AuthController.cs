using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using System.Threading.Tasks;
using UnrealEstate.Data.Entities;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Services.SendMail;
using UnrealEstate.Web.Services.Users;

namespace UnrealEstate.Web.Controllers.Api
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;
        private readonly IMailer _mailer;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUrlHelper _urlHelper;

        public AuthController(IUserService userService, UserManager<User> userManager, IMailer mailer, IHttpContextAccessor httpContextAccessor, IUrlHelper urlHelper)
        {
            _userService = userService;
            _userManager = userManager;
            _mailer = mailer;
            _httpContextAccessor = httpContextAccessor;
            _urlHelper = urlHelper;
        }

        [HttpPost]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _userService.AuthenticateAsync(request);

            if (string.IsNullOrEmpty(result.ResultObj))
            {
                return BadRequest(result);
            }

            return Ok(result);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            // || (await _userManager.IsEmailConfirmedAsync(user)
            if (user is null)
            {
                return BadRequest("Account does not exist");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(user);


            var callbackUrl = _urlHelper.Action(
                controller: "ForgotPassword",
                action: "ResetPassword",
                values: new { email = user.Email, token },
                protocol: _httpContextAccessor.HttpContext.Request.Scheme,
                host: "localhost:5001"
            );

            await _mailer.SenEmailAsync(user.Email, "Reset Password", callbackUrl);

            return Ok(token);
        }

        [HttpPost("reset-password", Name = "ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null)
            {
                return BadRequest("User does not exits!");
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);

            if (result.Succeeded)
            {
                return Ok();
            }

            return BadRequest("ResetPassword Failed!");
        }
    }
}
