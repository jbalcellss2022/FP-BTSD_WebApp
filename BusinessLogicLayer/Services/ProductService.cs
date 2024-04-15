using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace BusinessLogicLayer.Services
{
    internal class ProductService
    {
        private readonly IProductRepository<appProduct> productRepository;

        // Constructor & Dependency Injection //
        public ProductService(IProductRepository<appProduct> _productRepository)
        {
            productRepository = _productRepository;
        }

        // Business logic methods //
    }
}
