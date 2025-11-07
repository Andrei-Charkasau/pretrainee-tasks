using Inno_Shop_Cherkasov.DtoModels;
using Inno_Shop_Cherkasov.Models;

namespace Inno_Shop_Cherkasov.Services
{
    public interface IProductService
    {
        public Task CreateAsync(ProductDto productDto);
        public Task UpdateAsync(int productId, ProductDto productDto);
        public Task DeleteAsync(int productId);
        public Task<Product> GetAsync(int userId);
        public Task<List<Product>> GetAllAsync();
    }
}
