using Ecomm_demo.Data;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Ecomm_demo.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db) 
        {
            _db =db;
        }
        public async Task<Product> AddAsync(Product product)
        {
               _db.Products.Add(product);
              await _db.SaveChangesAsync();
            return product;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _db.Products.FindAsync(id);
            if(product == null)
            {
                return false;
            }
             _db.Products.Remove(product);
             await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Products.AnyAsync(p => p.ProductId ==id);
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
              return await   _db.Products.Include(p => p.Category)
                .Where(p => p.IsActive)
                .ToListAsync();
           
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
              return await _db.Products.Include(p => p.Category).ToListAsync();
        }

        public async Task<Product?> GetByIdAsync(int id)
        {
             return await _db.Products.Include(p => p.Category).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _db.Products
                .Include(p => p.Category)
                .ToListAsync();
        }

        public async Task<Product?> GetProductWithDetailsAsync(int productId)
        {
            return await _db.Products
                   .Include(p => p.Category)
                   .FirstOrDefaultAsync(p => p.ProductId == productId);
        }

        public async Task<Product> UpdateAsync(Product product)
        {
            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            return product;
        }

        public async Task<bool> UpdateStockAsync(int productId, int quantity)
        {
          var product =  await _db.Products.FindAsync(productId);
            if(product == null)
            {
                return false;
            }
            product.StockQuantity += quantity;
             await _db.SaveChangesAsync();
            return true;

        }
    }
}
