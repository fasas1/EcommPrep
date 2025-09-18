using Ecomm_demo.Entities;

namespace Ecomm_demo.Repository.IRepository
{
    public interface IOrderRepository :IRepository<Order>
    {
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        Task<IEnumerable<Order>> GetCustomerOrdersAsync(int customerId);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
    }
}
