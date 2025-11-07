using Inno_Shop_Cherkasov.DtoModels;
using Inno_Shop_Cherkasov.Models;
using Inno_Shop_Cherkasov.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Inno_Shop_Cherkasov.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, int> _productRepository;

        public ProductService(IRepository<Product, int> productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task CreateAsync(ProductDto productDto)
        {
            Product product = new Product()
            {
                Name = productDto.Name.Trim(),
                Description = productDto.Description.Trim(),
                Price = productDto.Price,
                CreationDate = productDto.CreationDate,
                CreatorId = productDto.CreatorId
            };
            await _productRepository.InsertAsync(product);
        }

        public async Task DeleteAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> GetAsync(int productId)
        {
            return await _productRepository.GetAsync(productId);
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAll().ToListAsync();
        }

        public async Task UpdateAsync(int productId, ProductDto productDto)
        {
            var existingProduct = await _productRepository.GetAsync(productId);

            Product patchProduct = new Product()
            {
                Id = productId,
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                CreationDate = productDto.CreationDate
            };

            await _productRepository.UpdateAsync(patchProduct);
        }
    }
}
