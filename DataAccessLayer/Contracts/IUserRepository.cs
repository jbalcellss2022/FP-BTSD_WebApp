using DataAccessLayer.Models;

namespace DataAccessLayer.Contracts
{
	public interface IUserRepository
	{
		/// <summary>
		/// Locate a user in the database.
		/// </summary>
		/// <param name="id">The id code of the product you want to obtain. </param>
		/// <returns>An appProduct object that has been located. Otherwise null.</returns>
		appUser? GetUserByEmail(string Username);

	}
}
