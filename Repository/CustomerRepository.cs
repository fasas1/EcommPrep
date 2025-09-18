using Ecomm_demo.Data;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Microsoft.EntityFrameworkCore;

namespace Ecomm_demo.Repository
{
    public class CustomerRepository :ICustomerRepository
    {
        private readonly ApplicationDbContext _db;
        public CustomerRepository(ApplicationDbContext db)
        { 
            _db = db;
        }

        public async Task<Customer> AddAsync(Customer customer)
        {
             _db.Customers.Add(customer);
              await _db.SaveChangesAsync();
              return customer;
        }

       
        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
             return await _db.Customers.ToListAsync();
        }
    
        public async Task<Customer?> GetByIdAsync(int id)
        {
            return await _db.Customers.FindAsync(id);
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer == null)
            {
                return false;
            }
             _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _db.Customers.AnyAsync(c => c.CustomerId == id);
        }
        public  async Task<Customer?> GetByEmailAsync(string email)
        {
            return await  _db.Customers.FirstOrDefaultAsync(c => c.Email == email);
        }

        public async Task<Customer?> GetCustomerWithOrdersAsync(int customerId)
        {
            return await  _db.Customers
                .Include(c => c.Orders)
                .FirstOrDefaultAsync(c =>c.CustomerId == customerId);

        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
             _db.Customers.Update(customer);
              await _db.SaveChangesAsync();
             return customer;
        }
    }
}
