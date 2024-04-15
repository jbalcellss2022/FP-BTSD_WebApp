using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class LoginController : Controller
    {
        private readonly IHttpContextAccessor httpContextAccessor;

        public LoginController(IHttpContextAccessor HttpContextAccessor)
        {
            httpContextAccessor = HttpContextAccessor;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var LoginUser = new AuthUser { UserId = "", Password = "" };
            var ProcesDebug = false;
            if (Debugger.IsAttached && ProcesDebug) // Debugger Login bypass
            {
                LoginUser.UserId = "admin@buttonwatch.com";
                LoginUser.Password = "43524410";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View("Login", LoginUser);
            }
        }
    }
}
