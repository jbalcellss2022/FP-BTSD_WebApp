using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Classes;
using DataAccessLayer.Contracts;
using DataAccessLayer.Models;
using static BCrypt.Net.BCrypt;

namespace BusinessLogicLayer.Services
{
	public class AuthService(IUserRepository UserRepository) : IAuthService
	{
		private readonly IUserRepository userRepository = UserRepository;

        public bool CheckUserAuth(LoginUserDTO loginUserDTO)
		{
			appUser? user = userRepository.GetUserByEmail(loginUserDTO.Username!);
			if (user != null) {
				if (Verify(loginUserDTO.Password, user.password))
				{
					return true;
				}
			}

			return false;
		}
	}
}
