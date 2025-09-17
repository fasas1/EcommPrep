using Ecomm_demo.Data;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Ecomm_demo.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _db;
        public CategoryRepository(ApplicationDbContext db)
        { 
            _db = db;
        }
        public async Task<Category> AddAsync(Category category)
        {
            _db.Categories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

      
        public async Task<IEnumerable<Category>> GetAllAsync()
        {
             return await _db.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _db.Categories.FindAsync();
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _db.Categories.FindAsync(id);
            if (category == null)
            {
                return false;
            }
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Categories.AnyAsync(c => c.CategoryId == id);
        }

        public async Task<IEnumerable<Category>> GetCategoriesWithProductCountAsync()
        {
                return await  _db.Categories
                .Include(c => c.Products)
                .ToListAsync();
        }

        public async Task<Category?> GetCategoryWithProductsAsync(int categoryId)
        {
             return await _db.Categories
                 .Include(c => c.Products)
                 .FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<Category> UpdateAsync(Category category)
        {
               _db.Categories.Update(category);
                await _db.SaveChangesAsync();
            return category;
        }
    }
}
