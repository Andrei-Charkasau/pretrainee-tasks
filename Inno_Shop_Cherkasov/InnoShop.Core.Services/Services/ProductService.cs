using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Repositories;
using InnoShop.Core.Services.Validators;
using Microsoft.EntityFrameworkCore;

namespace InnoShop.Core.Services.Services
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

            existingProduct.Id = productId;
            existingProduct.Name = productDto.Name.Trim();
            existingProduct.Description = productDto.Description.Trim();
            existingProduct.Price = productDto.Price;
            existingProduct.Availability = productDto.Availability;

            await _productRepository.UpdateAsync(existingProduct);
        }
    }
}
