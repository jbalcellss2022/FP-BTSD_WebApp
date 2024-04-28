using BusinessLogicLayer.Interfaces;
using BusinessLogicLayer.Services;
using DataAccessLayer.Repositories;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics.Internal;
using Microsoft.Extensions.Hosting.Internal;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using PresentationLayer;
using Resources;
using System;
using System.Globalization;
using System.Net;
using System.Reflection;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [Route("[controller]/[action]")]
    public class SignInController(
		IHttpContextAccessor httpContextAccessor, 
		IAuthService authService, 
		IStringLocalizer<BasicResources> LocalizeString,
		INotificationService notificationService,
        IWebHostEnvironment hostingEnvironment
        ) : Controller
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
				if (ModelState.IsValid || !loginUserDTO.AuthToken.IsNullOrEmpty())
				{
					ModelState.Clear();
                    if (loginUserDTO.AuthToken.IsNullOrEmpty())
                    {
                        if (authService.CheckUserAuth(loginUserDTO))
                        {
                            ClaimsIdentity identity = authService.CreateClaimsIdentity("pepe");
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
                        if (authService.CheckUserAuth(loginUserDTO))
                        {
                            ClaimsIdentity identity = authService.CreateClaimsIdentity("pepe");
                            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = loginUserDTO.KeepSigned });
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ModelState.Clear();
                            return View("NewAccount", loginUserDTO);
                        }
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

        [HttpGet]
        [AllowAnonymous]
        public IActionResult CreateAccount(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            return View("CreateAccount", loginUserDTO);
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateNewAccount(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            if (!authService.CanCreateNewAccount(loginUserDTO.Username!))
            {
                ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR2"]);
                return View("CreateAccount", loginUserDTO);
            }
            else
            {
                if (!authService.CreateNewAccount(loginUserDTO))
                {
                    ModelState.AddModelError(string.Empty, LocalizeString["LOGIN_ERROR2"]);
                    return View("CreateAccount", loginUserDTO);
                }
                else
                {
                    return View("", loginUserDTO);
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PasswordRecovery()
        {
            ModelState.Clear();
            return View("PasswordRecovery", new LoginUserDTO());
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordRecoverySentLink(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();

            // Send mail with token
            var newToken = "";
            var newTokenURL = "";

            newToken = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n");

            var request = httpContextAccessor.HttpContext!.Request;
            //newTokenURL = request.Scheme + "://" + request.Host.ToUriComponent() + "/Account/ResetPassword/" + WebUtility.UrlEncode(newToken);
            var rawurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(Request);
            var uri = new Uri(rawurl);
            newTokenURL = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped) + "/Account/ResetPassword/" + WebUtility.UrlEncode(newToken);
            /*
            authUser.TokenID = newToken;
            authUser.TokenIssuedUTC = DateTime.UtcNow;
            authUser.TokenExpiresUTC = DateTime.UtcNow.AddMinutes(15);
            ctxBBDD.DB_CfgCompanies.Update(authUser);
            ctxBBDD.SaveChanges();
            */
            var pathToFile = hostingEnvironment.WebRootPath
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "emailTemplates"
                                        + Path.DirectorySeparatorChar.ToString()
                                        + "resetpassword.html";
            var bodyHtml = new StringBuilder();
            using (StreamReader sourceReader = System.IO.File.OpenText(pathToFile))
            {
                bodyHtml.Append(sourceReader.ReadToEnd());
            }
            bodyHtml.Replace("{{username}}", loginUserDTO.Username);
            bodyHtml.Replace("{{userTokenURL}}", newTokenURL);

            await notificationService.EmailNotification("QRFY Support", loginUserDTO.Username!, "", "QRFY Password recovery", bodyHtml.ToString(), "");
            return View("PasswordRecoverySentLink", new LoginUserDTO());
        }

    }
}
