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
		IHttpContextAccessor HttpContextAccessor, 
		IAuthService AuthService, 
		IStringLocalizer<BasicResources> LocalizeString
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
                        if (AuthService.CheckUserAuth(loginUserDTO))
                        {
                            await DoLogin(loginUserDTO.Username!, loginUserDTO.KeepSigned);
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
                        if (AuthService.CheckUserAuth(loginUserDTO))
                        {
                            await DoLogin(loginUserDTO.Username!, loginUserDTO.KeepSigned);
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ModelState.Clear();
                            JWTDTO jwtToken = AuthService.GetJWTData(loginUserDTO.AuthToken!);
                            loginUserDTO.Username = jwtToken.Email;
                            loginUserDTO.Name = jwtToken.Name;

                            return RedirectToAction("CreateAccount", loginUserDTO);
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

        private async Task<bool> DoLogin(string UserName, bool KeepSigned)
        {
            try
            {
                ClaimsIdentity identity = AuthService.CreateClaimsIdentity(UserName!);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = KeepSigned });
                return true;
            }
            catch { }

            return false;
        }

        [HttpGet]
		[AllowAnonymous]
        public async Task<IActionResult> Logout()
		{
			// Clear the existing cookie to ensure a clean NEW login process
			if (HttpContextAccessor.HttpContext != null)
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
			if (HttpContextAccessor.HttpContext != null)
			{
				// Clear the existing cookie to ensure a clean NEW login process
				await HttpContextAccessor.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                foreach (var cookie in HttpContext.Request.Cookies) { 
                    Response.Cookies.Delete(cookie.Key); 
                }
                HttpContext.User = new GenericPrincipal(new GenericIdentity(string.Empty), null);
                await Task.Run(() => { 
					if (HttpContextAccessor.HttpContext.User.Identity != null)
						if (HttpContextAccessor.HttpContext.User.Identity.IsAuthenticated) { authBool = "1"; } 
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
        public async Task<IActionResult> CreateNewAccount(LoginUserDTO loginUserDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!AuthService.CanCreateNewAccount(loginUserDTO.Username!))
                    {
                        ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR1"], loginUserDTO.Username));
                        return View("CreateAccount", loginUserDTO);
                    }
                    else
                    {
                        if (!await AuthService.CreateNewAccount(loginUserDTO.Username!, loginUserDTO.Name!, loginUserDTO.Password!))
                        {
                            ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR2"], loginUserDTO.Username));
                            return View("CreateAccount", loginUserDTO);
                        }
                        else
                        {
                            await DoLogin(loginUserDTO.Username!, true);
                            return RedirectToAction("Index", "Dashboard");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR2"], loginUserDTO.Username));
                    return View("CreateAccount", loginUserDTO);
                }
            }
            catch (Exception) {
                ModelState.AddModelError(string.Empty, string.Format(LocalizeString["ACCOUNT_ERROR2"], loginUserDTO.Username));
                return View("CreateAccount", loginUserDTO);
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
        public async Task<IActionResult>PasswordRecoverySentLink(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            string newTokenURL = await AuthService.UserNewTokenResetPassword(loginUserDTO.Username!, HttpContextAccessor.HttpContext!.Request);
            return View("PasswordRecoverySentLink", new LoginUserDTO());
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult PasswordResetError()
        {
            return View("PasswordResetError");
        }

        [HttpGet("{userToken}")]
        [AllowAnonymous]
        public IActionResult PasswordReset(string userToken)
        {
            ModelState.Clear();
            bool result = AuthService.CheckUserToken(userToken);
            if (!result)
            {
                return RedirectToAction("PasswordResetError");
            }

            LoginUserDTO loginUserDTO = new()
            {
                AuthToken = userToken,
            };

            return View("PasswordChange", loginUserDTO);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PasswordChange(LoginUserDTO loginUserDTO)
        {
            ModelState.Clear();
            bool result = AuthService.CheckUserToken(loginUserDTO.AuthToken!);
            if (!result)
            {
                return RedirectToAction("PasswordResetError");
            }

            result = await AuthService.ChangePassword(loginUserDTO.AuthToken!, loginUserDTO.Password!);
            return View("PasswordChangeOk");
        }

    }
}
