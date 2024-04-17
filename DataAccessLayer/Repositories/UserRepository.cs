using DataAccessLayer.Contracts;
using DataAccessLayer.Data;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
	public class UserRepository(BBDDContext bbddcontext) : IUserRepository
	{
        /// <summary>
        /// Locate a user in the database.
        /// </summary>
        /// <param name="id">The id code of the product you want to obtain. </param>
        /// <returns>An appProduct object that has been located. Otherwise null.</returns>
        public appUser? GetUserByEmail(string Username)
        {
            if (bbddcontext != null && bbddcontext.appUsers != null)
            {
                appUser? user = bbddcontext.appUsers.FirstOrDefault(appUser => appUser.login == Username);
                return user;
            }

            return null;
        }
    }
}
