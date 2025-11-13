using Inno_Shop_Cherkasov.DtoModels;
using Inno_Shop_Cherkasov.Models;
using Inno_Shop_Cherkasov.Repositories;
using Inno_Shop_Cherkasov.Validators;
using Microsoft.EntityFrameworkCore;

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
            productDto.Name.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Product's NAME must be filled. !!!");
            productDto.Availability.ThrowExceptionIfNull("!!! ERROR: AVAILABILITY for product must be set. !!!");
            productDto.Price.ThrowExceptionIfNull("!!! ERROR:Product's PRICE must be set (NOT NULL). !!!");

            Product product = new Product()
            {
                Name = productDto.Name.Trim(),
                Description = productDto.Description.Trim(),
                Price = productDto.Price,
                CreationDate = productDto.CreationDate,
                Availability = productDto.Availability,
                CreatorId = productDto.CreatorId
            };
            await _productRepository.InsertAsync(product);
        }

        public async Task DeleteAsync(int productId)
        {
            var existingProduct = await _productRepository.GetAsync(productId);
            existingProduct.ThrowExceptionIfNull("ERROR: Product not found. !!!");
            await _productRepository.DeleteAsync(productId);
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
            productDto.Name.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Product's NAME must be filled. !!!");
            productDto.Availability.ThrowExceptionIfNull("!!! ERROR: AVAILABILITY for product must be set. !!!");
            productDto.Price.ThrowExceptionIfNull("!!! ERROR:Product's PRICE must be set (NOT NULL). !!!");
            var existingProduct = await _productRepository.GetAsync(productId);
            existingProduct.ThrowExceptionIfNull("!!! ERROR: Product not found. !!!");

            Product patchProduct = new Product()
            {
                Id = productId,
                Name = productDto.Name.Trim(),
                Description = productDto.Description.Trim(),
                Price = productDto.Price,
                Availability = productDto.Availability,
            };

            await _productRepository.UpdateAsync(patchProduct);
        }
    }
}
