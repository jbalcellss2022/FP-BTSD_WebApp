using BusinessLogicLayer.Interfaces;
using DataAccessLayer.Interfaces;
using Entities.Models;

namespace BusinessLogicLayer.Services
{
    public class ProductService(IProductRepository ProductRepository) : IProductService
    {
        public List<AppProduct> GetAllProducts()
        {
            var products = ProductRepository.GetAllProducts();
            return products;
        }

        public async Task<bool> AddProduct(AppProduct product)
        {
            bool result = false;
            try
            {
                result = await ProductRepository.AddProduct(product);
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> UpdateProduct(AppProduct product)
        {
            bool result = false;
            try
            {
                result = await ProductRepository.UpdateProduct(product);
            }
            catch (Exception)
            {
            }

            return result;
        }

        public async Task<bool> DeleteProduct(AppProduct product)
        {
            bool result = false;
            try
            {
                result = await ProductRepository.DeleteProduct(product);
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
