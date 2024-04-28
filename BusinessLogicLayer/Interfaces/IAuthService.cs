using Entities.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BusinessLogicLayer.Interfaces
{
	public interface IAuthService
	{
        /// <summary>
        /// Check if the user exists and the password is correct.
        /// </summary>
        /// <param name="loginUserDto">DTO of user login data</param>
        /// <returns>Returns True when authentication is successful. Otherwise returns False</returns>
        public bool CheckUserAuth(LoginUserDTO loginUserDTO);

        public bool CheckUserExist(LoginUserDTO loginUserDTO);

        public Task<bool> CheckUserIsBlocked(LoginUserDTO loginUserDTO);

        public Task<bool> IncreaseUserRetries(string Username);

        public Task<bool> ResetUserRetries(string Username);

        public JWTDTO GetJWTData(string jwtToken);

        public ClaimsIdentity CreateClaimsIdentity(string UserId);

        public bool CanCreateNewAccount(string Username);

        public Task<bool> CreateNewAccount(string Username, string Name, string Password);

        public Task<bool> ChangePassword(string Username, string Password);

        public Task<string> UserNewTokenResetPassword(string Username, HttpRequest request);

        public bool CheckUserToken(string token);
    }
}
