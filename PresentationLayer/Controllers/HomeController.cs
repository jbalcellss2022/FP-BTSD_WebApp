using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController()
        {

        }

        public IActionResult Index()
        {
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Clear the existing external cookie to ensure a clean login process
			return RedirectToAction("Index", "Home");

			return View();
        }
    }
}
