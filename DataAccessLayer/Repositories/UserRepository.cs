using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace DataAccessLayer.Repositories
{
	public class UserRepository : IUserRepository
	{
		/// <summary>
		/// Locate a user in the database.
		/// </summary>
		/// <param name="id">The id code of the product you want to obtain. </param>
		/// <returns>An appProduct object that has been located. Otherwise null.</returns>
		public appUser GetUserByEmail(string Username)
		{
			appUser user = new()
			{
				login = "pepe@pepe.com",
				password = "sdfssdfsdf"
			};

			return user;
		}
	}
}
