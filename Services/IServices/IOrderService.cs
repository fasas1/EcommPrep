using Ecomm_demo.Entities;

namespace Ecomm_demo.Services.IServices
{
    public interface IOrderService
    {
        Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto);
        Task<OrderDto?> GetOrderByIdAsync(int orderId);
        Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId);
        Task<bool> CancelOrderAsync(int orderId);
    }
}
