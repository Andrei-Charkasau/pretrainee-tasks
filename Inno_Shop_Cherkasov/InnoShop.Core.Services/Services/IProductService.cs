using InnoShop.Core.DtoModels;
using InnoShop.Core.Models;

namespace InnoShop.Core.Services.Services
{
    public interface IProductService
    {
        public Task CreateAsync(ProductDto productDto);
        public Task UpdateAsync(int productId, ProductDto productDto);
        public Task DeleteAsync(int productId);
        public Task<Product> GetAsync(int userId);
        public Task<List<Product>> GetAllAsync();
        public Task HideUserProductsAsync(int userId, int adminId);
        public Task ShowUserProductsAsync(int userId);
        public Task<bool> HideProductByAdminAsync(int productId, int adminId);
        public Task<bool> ShowProductByAdminAsync(int productId);
        public Task<List<Product>> SearchProductsAsync(ProductFilterDto filter);
    }
}
