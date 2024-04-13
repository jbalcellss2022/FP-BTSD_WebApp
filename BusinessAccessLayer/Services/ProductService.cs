using DataAccessLayer.Contracts;
using DataAccessLayer.Models;

namespace BusinessAccessLayer.Services
{
    internal class ProductService(IProductRepository<appProduct> _productRepository)
    {
        private readonly IProductRepository<appProduct> productRepository = _productRepository;

        // Business logic methods //

        void Dummysdfsdfsdf()
        {
            productRepository.GetById(1);
        }
    }
}
