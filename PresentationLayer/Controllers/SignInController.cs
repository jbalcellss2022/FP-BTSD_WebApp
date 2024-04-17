using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Classes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SignInController(IHttpContextAccessor httpContextAccessor, IAuthService authService) : Controller
    {
		[HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ModelState.Clear();
            return View("Login", new LoginUserDTO());
        }

		[HttpPost]
		[AllowAnonymous]
		public IActionResult Login(LoginUserDTO loginUserDTO)
		{
			if (ModelState.IsValid)
			{
                ModelState.Clear();
                if (authService.CheckUserAuth(loginUserDTO))
				{
					// TODO: Create and Get Claims from repository in BL
					var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
						identity.AddClaim(new Claim(ClaimTypes.Name, loginUserDTO.Username!));
						identity.AddClaim(new Claim("Usuario", loginUserDTO.Username!));
					// TODO: Use AuthorizeUSer in BL Service
					HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = loginUserDTO.KeepSigned });

					return RedirectToAction("Index", "Dashboard");
				}
				else
				{
                    ModelState.Clear();
                    ModelState.AddModelError(string.Empty, "Unable to log in, please check your login details.");
                    return View("Login", loginUserDTO);
				}
			}
			else
			{
				ModelState.Clear();
				ModelState.AddModelError(string.Empty, "Unable to log in, please check your login details.");
				return View("Login", loginUserDTO);
			}
		}

		[HttpGet]
		[AllowAnonymous]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); // Clear the existing external cookie to ensure a clean login process
			return RedirectToAction("Index", "Dashboard");
		}

		[HttpPost]
		[AllowAnonymous]
		public async Task<IActionResult> MakeLogout()
		{
			var authBool = "0";
			
			if (httpContextAccessor.HttpContext != null)
			{
                await httpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                foreach (var cookie in HttpContext.Request.Cookies) { Response.Cookies.Delete(cookie.Key); }
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                await Task.Run(() => { 
					if (httpContextAccessor.HttpContext.User.Identity != null)
						if (httpContextAccessor.HttpContext.User.Identity.IsAuthenticated) { authBool = "1"; } 
				});

                return StatusCode(StatusCodes.Status200OK, authBool);
            }
			else return StatusCode(StatusCodes.Status400BadRequest, authBool);
        }

	}
}
