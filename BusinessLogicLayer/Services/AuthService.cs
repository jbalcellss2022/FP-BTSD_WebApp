using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;

namespace BusinessLogicLayer.Services
{
    public class AuthService(IUserRepository userRepository, IEncryptionService encryption) : IAuthService
	{
        public bool CheckUserAuth(LoginUserDTO loginUserDTO)
		{
			AppUser? user = userRepository.GetUserByEmail(loginUserDTO.Username!);
			if (user != null) {
				if (encryption.CheckBCryptPassword(loginUserDTO.Password!, user.Password))
				{
                    return true;
				}
			}

			return false;
		}
    }
}
