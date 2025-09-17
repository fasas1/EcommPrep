using Ecomm_demo.Entities;

namespace Ecomm_demo.Repository.IRepository
{
    public interface IProductRepository :IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<Product?> GetProductWithDetailsAsync(int productId);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
        Task<bool> UpdateStockAsync(int productId, int quantity);
    }

}
