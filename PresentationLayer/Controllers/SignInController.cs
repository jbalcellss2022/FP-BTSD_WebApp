using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using Entities.DTOs;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Localization;
using PresentationLayer;
using Resources;
using System.Globalization;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SignInController(IHttpContextAccessor httpContextAccessor, IAuthService authService, IUserDDService userDDService, IStringLocalizer<BasicResources> LocalizeString) : Controller
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
        public async Task<IActionResult> Login(LoginUserDTO loginUserDTO)
		{
			try
			{
				if (ModelState.IsValid)
				{
					ModelState.Clear();
					if (authService.CheckUserAuth(loginUserDTO))
					{
                        await userDDService.AddUserDeviceDetector(loginUserDTO.Username!);

                        // TODO: Create and Get Claims from repository in BL
                        var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
						identity.AddClaim(new Claim(ClaimTypes.Name, loginUserDTO.Username!));
						identity.AddClaim(new Claim("Usuario", loginUserDTO.Username!));
						// TODO: Use AuthorizeUSer in BL Service
						await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = loginUserDTO.KeepSigned });

						return RedirectToAction("Index", "Dashboard");
					}
					else
					{
						ModelState.Clear();
						ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR1"]);
						return View("Login", loginUserDTO);
					}
                }
				else
				{
					ModelState.Clear();
					ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR2"]);
					return View("Login", loginUserDTO);
				}
			}
			catch (Exception) {
				ModelState.Clear();
				ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR2"]);
				return View("Login", loginUserDTO);
			}
		}

		[HttpGet]
		[AllowAnonymous]
        public async Task<IActionResult> Logout()
		{
			// Clear the existing cookie to ensure a clean NEW login process
			if (httpContextAccessor.HttpContext != null)
			{
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return RedirectToAction("Index", "Dashboard");
			}
			else return StatusCode(StatusCodes.Status400BadRequest);
		}

		[HttpPost]
		[AllowAnonymous]
        public async Task<IActionResult> MakeLogout()
		{
			var authBool = "0";
			if (httpContextAccessor.HttpContext != null)
			{
				// Clear the existing cookie to ensure a clean NEW login process
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
