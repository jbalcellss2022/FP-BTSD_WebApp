using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
    internal class ProductService
    {
        private readonly IProductRepository<AppProduct> productRepository;

        public ProductService(IProductRepository<AppProduct> _productRepository)
        {
            productRepository = _productRepository;
        }

        // Business logic methods 
        // ... //
    }
}
