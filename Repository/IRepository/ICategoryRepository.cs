using Ecomm_demo.Entities;

namespace Ecomm_demo.Repository.IRepository
{
    public interface ICategoryRepository :IRepository<Category>
    {
        Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync();
        Task<Category?> GetCategoryWithProductsAsync(int categoryId);
    }
}
