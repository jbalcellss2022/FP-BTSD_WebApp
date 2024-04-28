using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BusinessLogicLayer.Services
{
    public class AuthService(
        IUserRepository UserRepository, 
        IEncryptionService Encryption, 
        IUserDDService UserDDService,
        INotificationService NotificationService,
        IHelpersService EmailBodyHelper
        ) : IAuthService{

        public bool CheckUserAuth(LoginUserDTO loginUserDTO)
		{
            if (!loginUserDTO.AuthToken.IsNullOrEmpty())
			{
                bool result = false;

                // Check user by OAuth2 Token
                JwtSecurityToken jwtToken = Encryption.JWT_CheckExternalToken(loginUserDTO.AuthToken!);
                if (jwtToken != null)
                {
                    string email = Encryption.JWT_GetEmailFromToken(jwtToken);
                    AppUser? user = UserRepository.GetUserByEmail(email);
                    if (user != null && !Encryption.JWT_IsExpired(jwtToken))
                    {
                        UserDDService.AddUserDeviceDetector(user.Login);
                        loginUserDTO.Username = email;
                        result = true;
                    }
                }

                return result;
            }
			else
			{
                // Check user by username & password
                AppUser? user = UserRepository.GetUserByEmail(loginUserDTO.Username!);
                if (user != null)
                   return Encryption.BCrypt_CheckPassword(loginUserDTO.Password!, user.Password!);
            }

			return false;
		}

        public JWTDTO GetJWTData(string token)
        {
            JWTDTO? JWT = null;
            try
            {
                JwtSecurityToken jwtToken = Encryption.JWT_CheckExternalToken(token);
                JWT = Encryption.JWT_GetDataFromToken(jwtToken);
            }
            catch { }

            return JWT!;
        }

        public ClaimsIdentity CreateClaimsIdentity(string UserId)
        {
            ClaimsIdentity? identity = null;
            try
            {
                identity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, UserId));
                identity.AddClaim(new Claim("UserId", UserId));
            }
            catch { }   

            return identity!;
        }

        public bool CanCreateNewAccount(string Username)
        {
            AppUser? user = UserRepository.GetUserByEmail(Username);
            return (user == null);
        }

        public async Task<bool> CreateNewAccount(string Username, string Name, string Password)
        {
            bool result = false;
            try
            {
                string EncryptedPassword = Encryption.BCrypt_EncryptPassword(Password);
                result = await UserRepository.CreateAccount(Username, Name, EncryptedPassword);
                if (result) {
                    StringBuilder bodyHtml = EmailBodyHelper.EmailBodyAccount_Welcome(Username, Name);
                    await NotificationService.EmailNotification("QRFY Support", Username, "", "Welcome to QRFY!", bodyHtml.ToString(), "");
                }
            }
            catch { }

            return result;
        }

        public async Task<string> UserNewTokenResetPassword(string Username, HttpRequest request)
        {
            try
            {
                var newToken = GetNewTokenResetPassword();
                var newTokenURL = GetNewURLTokenResetPassword(newToken, request);
                await UserRepository.UpdateUserToken(Username, newToken);

                StringBuilder bodyHtml = EmailBodyHelper.EmailBodyPassword_New(Username, newTokenURL);
                await NotificationService.EmailNotification("QRFY Support", Username, "", "QRFY Password recovery", bodyHtml.ToString(), "");

                return newTokenURL;
            }
            catch { }

            return "";
        }

        public bool CheckUserToken(string token)
        {
            try
            {
                AppUser? user = UserRepository.GetUserByToken(token!);
                if (user != null)
                {
                    if (user.TokenIsValid == true && DateTime.UtcNow <= user.TokenExpiresUTC)
                    {
                        return true;
                    }
                    else return false;
                }
            }
            catch { }

            return false;
        }

        public async Task<bool> ChangePassword(string AuthToken, string Password)
        {
            try
            {
                AppUser? user = UserRepository.GetUserByToken(AuthToken);
                if (user != null)
                {
                    bool result = await UserRepository.UpdateUserPassword(user.UserId, Encryption.BCrypt_EncryptPassword(Password));
                    if (result)
                    {
                        StringBuilder bodyHtml = EmailBodyHelper.EmailBodyPassword_Change(user.Login);
                        await NotificationService.EmailNotification("QRFY Support", user.Login, "", "QRFY Password changed", bodyHtml.ToString(), "");

                        return true;
                    }
                }
            }
            catch { }

            return false;
        }


        //############################################################################################################//
        //############################################## PRIVATE METHODS ##############################################//
        //############################################################################################################//

        private static string GetNewTokenResetPassword()
        {
            var newToken = Guid.NewGuid().ToString("n") + Guid.NewGuid().ToString("n");
            return newToken;
        }

        private static string GetNewURLTokenResetPassword(string newToken, HttpRequest request)
        {
            string newTokenURL = "";
            try
            {
                var rawurl = Microsoft.AspNetCore.Http.Extensions.UriHelper.GetDisplayUrl(request);
                var uri = new Uri(rawurl);
                newTokenURL = uri.GetComponents(UriComponents.Scheme | UriComponents.Host | UriComponents.Port, UriFormat.UriEscaped) + "/SignIn/PasswordReset/" + WebUtility.UrlEncode(newToken);
                return newTokenURL;
            }
            catch { }

            return newTokenURL;
        }
    }
}
