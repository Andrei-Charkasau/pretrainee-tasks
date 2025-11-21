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
            var product = await _productRepository.GetAsync(productId);
            if (product == null || product.IsHidden) { return null; } //Если скрыт - возвращаю null... Пока так.
            return product;
        }

        public async Task<List<Product>> GetAllAsync()
        {
            return await _productRepository.GetAll().Where(p => !p.IsHidden).ToListAsync();
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

        public async Task<List<Product>> GetUserProductsAsync(int userId)
        {
            return await _productRepository.GetAll().Where(p => p.CreatorId == userId &&
                                                               !p.IsHidden).ToListAsync();
        }

        public async Task HideUserProductsAsync(int userId, int adminId) //Скрываем ВСЕ продукты пользователя (прячем, Hide)
        {
            var userProducts = await _productRepository.GetAll().Where(p => p.CreatorId == userId &&
                                                                           !p.IsHidden).ToListAsync();

            foreach (var product in userProducts)
            {
                product.IsHidden = true;
                product.HiddenAt = DateTime.UtcNow;
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task ShowUserProductsAsync(int userId) //Показываем ВСЕ продукты пользователя (типа Unhide)
        {
            var userProducts = await _productRepository.GetAll().Where(p => p.CreatorId == userId &&
                                                                            p.IsHidden).ToListAsync();

            foreach (var product in userProducts)
            {
                product.IsHidden = false;
                product.HiddenAt = null;
                await _productRepository.UpdateAsync(product);
            }
        }

        public async Task<bool> HideProductByAdminAsync(int productId, int adminId) //Скрываем продукт (прячем, Hide) / для админа
        {
            var product = await _productRepository.GetAsync(productId);
            if (product == null) return false;

            product.IsHidden = true;
            product.HiddenAt = DateTime.UtcNow;

            await _productRepository.UpdateAsync(product);
            return true;
        }

        public async Task<bool> ShowProductByAdminAsync(int productId) //"Активируем" продукт (типа Unhide) / для админа
        {
            var product = await _productRepository.GetAsync(productId);
            if (product == null) return false;

            product.IsHidden = false;
            product.HiddenAt = null;

            await _productRepository.UpdateAsync(product);
            return true;
        }
    }
}
