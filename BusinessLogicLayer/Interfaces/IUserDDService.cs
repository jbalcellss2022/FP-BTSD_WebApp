using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IUserDDService
    {
        /// <summary>
        /// Starts the location database by IP address.
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public void StartDeviceDetector();

        /// <summary>
        /// Retrieves non-private device information from which a user logs in and adds it to the user logger database.
        /// </summary>
        /// <param name="userId">Username value as a string.</param>
        /// <returns></returns>
        public Task<bool> AddUserDeviceDetector(string? userId);

    }
}
