using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using static BCrypt.Net.BCrypt;

namespace BusinessLogicLayer.Services
{
	public class AuthService(IUserRepository UserRepository) : IAuthService
	{
		private readonly IUserRepository userRepository = UserRepository;

        public bool CheckUserAuth(LoginUserDTO loginUserDTO)
		{
			AppUser? user = userRepository.GetUserByEmail(loginUserDTO.Username!);
			if (user != null) {
				if (Verify(loginUserDTO.Password, user.Password))
				{
					return true;
				}
			}

			return false;
		}
	}
}
