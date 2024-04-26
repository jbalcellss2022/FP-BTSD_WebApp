using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;
using Entities.Models;
using Microsoft.IdentityModel.Tokens;

namespace BusinessLogicLayer.Services
{
    public class AuthService(IUserRepository userRepository, IEncryptionService encryption) : IAuthService
	{
        public bool CheckUserAuth(LoginUserDTO loginUserDTO)
		{
			if (!loginUserDTO.AuthToken.IsNullOrEmpty())
			{

			}
			string UserNameEmail = loginUserDTO.Username!;

            AppUser? user = userRepository.GetUserByEmail(UserNameEmail);
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
