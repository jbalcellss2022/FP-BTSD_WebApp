using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;

using Entities.Models;

namespace BusinessLogicLayer.Services
{
    public class UserService(IUserRepository UserRepository) : IUserService
    {
        public List<AppUsersStat> GetAllUserStats(string Username) { 
            return UserRepository.GetAllUserStats(Username);
        }
    }
}
