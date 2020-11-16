using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UnrealEstate.ViewModels.Catalog.Users;
using UnrealEstate.Web.Controllers.Api;

namespace UnrealEstate.Web.Controllers
{
    public class ForgotPasswordController : Controller
    {
        private readonly AuthController _authController;

        public ForgotPasswordController(AuthController authController)
        {
            _authController = authController;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(ForgotPasswordModel request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "ForgotPassword Failed!");
                return View(request);
            }

            var result = (ObjectResult) _authController.ForgotPassword(request).Result;

            if (result.StatusCode.Value == 200)
            {
                ModelState.AddModelError(string.Empty, "Success");
                return View(request);
            }

            ModelState.AddModelError(string.Empty, result.ToString());

            return View(request);
        }

        [HttpGet]
        public IActionResult ResetPassword(string token, string email)
        {
            if (token is null || email is null)
            {
                return RedirectToAction("Index", "Listing");
            }

            var model = new ResetPasswordModel { Token = token, Email = email };

            return View(model);
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordModel request)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError(string.Empty, "ResetPassword Failed!");

                return View(request);
            }

            var result = (ObjectResult)_authController.ResetPassword(request).Result;

            if (result.StatusCode.Value == 200)
            {
                return RedirectToAction("Index", "Login");
            }

            ModelState.AddModelError(string.Empty, result.Value.ToString());

            return View(request);
        }
    }
}
