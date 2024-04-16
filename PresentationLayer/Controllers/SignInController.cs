using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
using System.Security.Principal;

namespace PresentationLayer.Controllers
{
	[Authorize]
    [Route("[controller]/[action]")]
    public class SignInController(IHttpContextAccessor httpContextAccessor, IAuthService AuthService) : Controller
    {
		[HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            var ProcesDebug = false;
            if (Debugger.IsAttached && ProcesDebug) // Debugger Login bypass
            {
				AuthUserDTO authUser = new() {
					Username = "admin@qrfy.es",
					Password = "43524410",
					KeepSigned = true
				};
				return RedirectToAction("Index", "Home");
            }
            else return View("Login", new AuthUserDTO());
        }

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login(AuthUserDTO authUser)
		{
			if (ModelState.IsValid)
			{
				if (AuthService.CheckUserAuth(authUser) != null)
				{
					var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
					identity.AddClaim(new Claim(ClaimTypes.Name, authUser.Username));
					identity.AddClaim(new Claim("Usuario", authUser.Username));
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = authUser.KeepSigned });
					return RedirectToAction("Index", "Home");
				}
				else
				{
					return View();
				}
			}
			else
			{
				return View();
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Clear the existing external cookie to ensure a clean login process
			return RedirectToAction("Index", "Home");
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> MakeLogout()
		{
			var authBool = "0";
			
			await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			foreach (var cookie in HttpContext.Request.Cookies) { Response.Cookies.Delete(cookie.Key); }
			HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
			await Task.Run(() => { if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) { authBool = "1"; } });

			return StatusCode(200, authBool);
		}

	}
}
