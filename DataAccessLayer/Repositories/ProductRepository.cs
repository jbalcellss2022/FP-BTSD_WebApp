using DataAccessLayer.Interfaces;
using Entities.Data;
using Entities.Models;

namespace DataAccessLayer.Repositories
{
    public class ProductRepository(BBDDContext bbddcontext) : IProductRepository
    {
        public List<AppProduct> GetAllProducts()
        {
            var products = bbddcontext.AppProducts.ToList();
            return products;
        }

        public async Task<bool> AddProduct(AppProduct product)
        {
            bool result = false;
            try
            {
                bbddcontext.AppProducts.Add(product);
                await bbddcontext.SaveChangesAsync();
                result = true;
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
                bbddcontext.AppProducts.Update(product);
                await bbddcontext.SaveChangesAsync();
                result = true;
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
                bbddcontext.AppProducts.Remove(product);
                await bbddcontext.SaveChangesAsync();
                result = true;
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
