using DataAccessLayer.Classes;
using DataAccessLayer.Models;

namespace DataAccessLayer.Contracts
{
	public interface IUserRepository
	{
        /// <summary>
        /// Locate a user by email in the database.
        /// </summary>
        /// <param name="Username">The user email. </param>
        /// <returns>An model class product that has been located. Otherwise null.</returns>
        public Task<appUser?> GetUserByEmail(string Username);

        /// <summary>
        /// Locate a user by email in the database.
        /// </summary>
        /// <param name="Username">The user email. </param>
        /// <returns>The Guid of the user located. Otherwise empty GUID.</returns>
        public Guid GetUserIdByEmail(string Username);

        /// <summary>
        /// Adds user device data to the database
        /// </summary>
        /// <param name="userDDDTO">The user device data DTO. </param>
        /// <returns>Object of type Task asynchronous with the number of insertions.</returns>
        public Task<bool> AddUserDD(UserDDDTO userDDDTO);
    }
}
