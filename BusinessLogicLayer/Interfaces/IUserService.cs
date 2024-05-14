using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserService
    {
        /// <summary>
        /// Get all user stats
        /// </summary>
        /// <returns></returns>
        public List<AppUsersStat> GetAllUserStats(string Username);
    }

}
