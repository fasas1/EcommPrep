using Ecomm_demo.Data;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Ecomm_demo.Repository
{
    public class OrderRepository :IOrderRepository
    {
        private readonly ApplicationDbContext _db;

        public OrderRepository(ApplicationDbContext db)
        {
            _db = db;
        }


        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await _db.Orders.ToListAsync();
        }

        public async Task<Order?> GetByIdAsync(int id)
        {
            return await _db.Orders.FindAsync(id);
        }
        public async Task<Order> AddAsync(Order entity)
        {
             _db.Orders.Add(entity);
             await _db.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order =  await _db.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }
             _db.Orders.Remove(order);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Orders.AnyAsync(o => o.OrderId == id);
        }

        public async Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId)
        {
            throw new NotImplementedException();

        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            return await _db.Orders.Include(o => o.Customer).Where(o => o.Status == status).ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
          
            return await _db.Orders
              .Include(o => o.Customer)
              .Include(o => o.OrderItems)
               .ThenInclude(o => o.Product)
               .ThenInclude(o => o.Category)
                 .FirstOrDefaultAsync(o => o.OrderId == orderId);
        }

        public Task<Order> UpdateAsync(Order entity)
        {
            throw new NotImplementedException();
        }
    }
}
