using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
<<<<<<< HEAD
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
=======
    internal class ProductService(IProductRepository<appProduct> _productRepository)
    {
        private readonly IProductRepository<appProduct> productRepository = _productRepository;

        // Business logic methods //

        void Dummysdfsdfsdf()
>>>>>>> b5fa13c8c466b416b8741726933d7f3b1a40e1f0
        {
            productRepository.GetById(1);
        }
    }
}
