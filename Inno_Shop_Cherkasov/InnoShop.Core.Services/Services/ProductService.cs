using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;
using InnoShop.Core.Repositories.Repositories;
using InnoShop.Core.Services.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace InnoShop.Core.Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<Product, int> _productRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductService(IRepository<Product, int> productRepository, IHttpContextAccessor httpContextAccessor)
        {
            _productRepository = productRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateAsync(ProductDto productDto)
        {
            productDto.Name.ThrowExceptionIfNullOrWhiteSpace("!!! ERROR: Product's NAME must be filled. !!!");
            productDto.Availability.ThrowExceptionIfNull("!!! ERROR: AVAILABILITY for product must be set. !!!");

            var currentUserId = GetCurrentUserId();

            Product product = new Product()
            {
                Name = productDto.Name.Trim(),
                Description = productDto.Description.Trim(),
                Price = productDto.Price,
                CreationDate = DateTime.Now,
                Availability = productDto.Availability,
                CreatorId = currentUserId
            };
            await _productRepository.InsertAsync(product);
        }

        public async Task DeleteAsync(int productId)
        {
            var existingProduct = await _productRepository.GetAsync(productId);
            existingProduct.ThrowExceptionIfNull("ERROR: Product not found. !!!");
            if (!await IsProductOwnerAsync(productId))
            {
                throw new UnauthorizedAccessException("!!! ERROR: You can only delete YOUR own products. !!!");
            }
            await _productRepository.DeleteAsync(productId);
        }

        public async Task<Product?> GetAsync(int productId)
        {
            var product = await _productRepository.GetAsync(productId);
            if (product == null || product.IsHidden) 
            { 
                return null; 
            }
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
            var existingProduct = await _productRepository.GetAsync(productId);
            existingProduct.ThrowExceptionIfNull("!!! ERROR: Product not found. !!!");
            if (!await IsProductOwnerAsync(productId))
            {
                throw new UnauthorizedAccessException("!!! ERROR: You can only edit your own products. !!!");
            }

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

        public async Task<List<Product>> SearchProductsAsync(ProductFilterDto filter)
        {
            var query = _productRepository.GetAll().Where(p => !p.IsHidden);
            query = ApplyFilters(query, filter);
            query.OrderByDescending(p => p.CreationDate);

            return await query.ToListAsync();
        }

        private IQueryable<Product> ApplyFilters(IQueryable<Product> query, ProductFilterDto filter)
        {
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm)) //+Фильтр "по названию/описанию"
            {
                query = query.Where(p => p.Name.Contains(filter.SearchTerm) || p.Description.Contains(filter.SearchTerm));
            }

            query = query.Where(product =>
                (!filter.MinPrice.HasValue || product.Price >= filter.MinPrice.Value) &&                   //+Фильтр "по цене (нижний предел)"
                (!filter.MaxPrice.HasValue || product.Price <= filter.MaxPrice.Value) &&                   //+Фильтр "по цене (верхний предел)"
                (!filter.Availability.HasValue || product.Availability == filter.Availability.Value) &&    //+Фильтр "по доступности (наличию)"
                (!filter.CreatedAfter.HasValue || product.CreationDate >= filter.CreatedAfter.Value) &&    //+Фильтр "дате создание (После)"
                (!filter.CreatedBefore.HasValue || product.CreationDate <= filter.CreatedBefore.Value) &&  //+Фильтр "дате создание (До)"
                (!filter.CreatorId.HasValue || product.CreatorId == filter.CreatorId.Value));              //+Фильтр "по создателю"

            return query;
        }

        private int GetCurrentUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return int.Parse(userIdClaim ?? "0");
        }

        private async Task<bool> IsProductOwnerAsync(int productId)
        {
            var product = await _productRepository.GetAsync(productId);
            var currentUserId = GetCurrentUserId();
            var isAdmin = _httpContextAccessor.HttpContext?.User.IsInRole("Admin") == true;

            return product?.CreatorId == currentUserId || isAdmin;
        }
    }
}
