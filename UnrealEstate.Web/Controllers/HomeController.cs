using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace UnrealEstate.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(string email, string token)
        {
            return View();
        }
    }
}
