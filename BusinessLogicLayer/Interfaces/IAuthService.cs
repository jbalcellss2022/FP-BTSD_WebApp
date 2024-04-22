using Entities.DTOs;

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

	}
}
