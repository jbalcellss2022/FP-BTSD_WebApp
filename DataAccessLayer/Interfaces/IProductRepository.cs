using Entities.Models;

namespace DataAccessLayer.Interfaces
{
    public interface IProductRepository
    {
        /// <summary>
        /// Locate a product in the database by its product id reference.
        /// </summary>
        /// <param name="id">The id code of the product you want to obtain. </param>
        /// <returns>An AppProduct object that has been located. Otherwise null.</returns>
    }
}
