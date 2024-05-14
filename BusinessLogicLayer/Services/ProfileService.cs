using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.DTOs;

namespace BusinessLogicLayer.Services
{
    public class ProfileService(IUserRepository UserRepository) : IProfileService
    {
        public DashboardUserProfileDTO GetUserProfile(string username)
        {
            DashboardUserProfileDTO userProfile = UserRepository.GetUserProfile(username);
            return userProfile;
        }
    }
}
