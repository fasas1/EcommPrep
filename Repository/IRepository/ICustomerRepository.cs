using Ecomm_demo.Entities;

namespace Ecomm_demo.Repository.IRepository
{
    public interface ICustomerRepository :IRepository<Customer>
    {
        Task<Customer?> GetCustomerWithOrdersAsync(int customerId);
        Task<Customer?> GetByEmailAsync(string email);
    }
}
