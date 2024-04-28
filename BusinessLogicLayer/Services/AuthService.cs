using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace BusinessLogicLayer.Services
{
    public class AuthService(IUserRepository userRepository, IEncryptionService encryption, IUserDDService userDDService) : IAuthService
	{
        public bool CheckUserAuth(LoginUserDTO loginUserDTO)
		{
            if (!loginUserDTO.AuthToken.IsNullOrEmpty())
			{
                bool result = false;

                // Check user by OAuth2 Token
                JwtSecurityToken jwtToken = encryption.CheckJWTExternalToken(loginUserDTO.AuthToken!);
                if (jwtToken != null)
                {
                    string email = encryption.GetJWTEmailFromToken(jwtToken);
                    AppUser? user = userRepository.GetUserByEmail(email);
                    if (user != null && !encryption.IsExpiredJWT(jwtToken))
                    {
                        userDDService.AddUserDeviceDetector(user.Login);
                        loginUserDTO.Username = email;
                        result = true;
                    }
                }

                return result;
            }
			else
			{
                // Check user by username & password
                AppUser? user = userRepository.GetUserByEmail(loginUserDTO.Username!);
                if (user != null)
                   return encryption.CheckBCryptPassword(loginUserDTO.Password!, user.Password);
            }

			return false;
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
            AppUser? user = userRepository.GetUserByEmail(Username);
            return (user == null);
        }

        public bool CreateNewAccount(LoginUserDTO loginUserDTO)
        {
            bool result = false;



            return result;
        }
    }
}
