using Entities.DTOs;

namespace BusinessLogicLayer.Interfaces
{
    public interface IProfileService
    {
        /// <summary>
        /// Get the user profile
        /// </summary>
        /// <param name="username"></param>
        /// <returns>Returns a DTO with all user profile data</returns>
        public DashboardUserProfileDTO GetUserProfile(string username);
    }
}
