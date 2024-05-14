using Entities.Models;

namespace BusinessLogicLayer.Interfaces
{
    public interface IProductService
    {
        /// <summary>
        /// Get all products from the database.
        /// </summary>
        /// <returns></returns>
        List<AppProduct> GetAllProducts();

        /// <summary>
        /// Add a product to the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> AddProduct(AppProduct product);

        /// <summary>
        /// Update a product in the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> UpdateProduct(AppProduct product);

        /// <summary>
        /// Delete a product from the database.
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        Task<bool> DeleteProduct(AppProduct product);
    }
}
