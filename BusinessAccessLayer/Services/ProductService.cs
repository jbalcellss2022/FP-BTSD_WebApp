using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
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

        void dummy()
        {
            productRepository.GetById(1);
        }
    }
}
