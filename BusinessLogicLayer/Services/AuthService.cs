using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Classes;
using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
	public class AuthService : IAuthService
	{
		private readonly IUserRepository userRepository;

		public AuthService(IUserRepository UserRepository)
		{
			userRepository = UserRepository;
		}

		public UserDTO CheckUserAuth(AuthUserDTO loginUserDTO)
		{
			UserDTO userDTO = null;

			string hash1 = BCrypt.Net.BCrypt.HashPassword("S43524410s");
			string hash2 = BCrypt.Net.BCrypt.HashPassword("qrfydemo2024");

			appUser user = userRepository.GetUserByEmail(loginUserDTO.Username);
			if (user != null) {
				if (BCrypt.Net.BCrypt.Verify(loginUserDTO.Password, hash1))
				{
					userDTO = new(){ 
						Username = user.login,
						KeepSigned = loginUserDTO.KeepSigned,
						isAdmin = true
					};
				}
			}

			return userDTO;
		}
	}
}
