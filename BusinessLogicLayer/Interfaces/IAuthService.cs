using DataAccessLayer.Classes;

namespace BusinessLogicLayer.Interfaces
{
	public interface IAuthService
	{
		/// <summary>
		/// Check if the user exists and the password is correct.
		/// </summary>
		/// <param name="loginUserDto"></param>
		/// <returns>Returns an object of type UserDTO if authentication is successful. Otherwise it returns null</returns>
		bool CheckUserAuth(LoginUserDTO loginUserDTO);

	}
}
