using AutoMapper;
using Ecomm_demo.Data;
using Ecomm_demo.Entities;
using Ecomm_demo.Repository.IRepository;
using Ecomm_demo.Services.IServices;

namespace Ecomm_demo.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _db;
        private readonly ILogger<OrderService> _logger;

        public OrderService(
            IOrderRepository orderRepository,
            IProductRepository productRepository,
            ICustomerRepository customerRepository,
            IMapper mapper,
            ApplicationDbContext db,
            ILogger<OrderService> logger)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _db = db;
            _logger = logger;
        }

        public async Task<OrderDto> CreateOrderAsync(CreateOrderDto createOrderDto)
        {
            _logger.LogInformation("Creating order for customer {CustomerId}", createOrderDto.CustomerId);

            // Validation
            if (!await _customerRepository.ExistsAsync(createOrderDto.CustomerId))
            {
                throw new InvalidOperationException("Customer not found");
            }

            if (!createOrderDto.Items.Any())
            {
                throw new InvalidOperationException("Order must have at least one item");
            }

            // Start transaction for complex operation
            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // Calculate order totals
                decimal subTotal = 0;
                var orderItems = new List<OrderItem>();

                foreach (var itemDto in createOrderDto.Items)
                {
                    var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                    if (product == null)
                    {
                        throw new InvalidOperationException($"Product {itemDto.ProductId} not found");
                    }

                    if (product.StockQuantity < itemDto.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                    }

                    var lineTotal = product.Price * itemDto.Quantity;
                    subTotal += lineTotal;

                    orderItems.Add(new OrderItem
                    {
                        ProductId = itemDto.ProductId,
                        Quantity = itemDto.Quantity,
                        UnitPrice = product.Price,
                        LineTotal = lineTotal
                    });

                    // Reduce stock
                    await _productRepository.UpdateStockAsync(itemDto.ProductId, -itemDto.Quantity);
                }

                var tax = subTotal * 0.1m; // 10% tax
                var total = subTotal + tax;

                // Create order
                var order = new Order
                {
                    CustomerId = createOrderDto.CustomerId,
                    SubTotal = subTotal,
                    Tax = tax,
                    Total = total,
                    Status = OrderStatus.Pending,
                    OrderItems = orderItems
                };

                var createdOrder = await _orderRepository.AddAsync(order);
                await transaction.CommitAsync();

                // Return with full details
                var orderWithDetails = await _orderRepository.GetOrderWithDetailsAsync(createdOrder.OrderId);
                return _mapper.Map<OrderDto>(orderWithDetails!);
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<OrderDto?> GetOrderByIdAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            return order == null ? null : _mapper.Map<OrderDto>(order);
        }

        public async Task<IEnumerable<OrderDto>> GetCustomerOrdersAsync(int customerId)
        {
            var orders = await _orderRepository.GetCustomerOrdersAsync(customerId);
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<bool> CancelOrderAsync(int orderId)
        {
            var order = await _orderRepository.GetOrderWithDetailsAsync(orderId);
            if (order == null) return false;

            if (order.Status != OrderStatus.Pending)
            {
                throw new InvalidOperationException("Can only cancel pending orders");
            }

            using var transaction = await _db.Database.BeginTransactionAsync();

            try
            {
                // Restore stock
                foreach (var item in order.OrderItems)
                {
                    await _productRepository.UpdateStockAsync(item.ProductId, item.Quantity);
                }

                // Update order status
                order.Status = OrderStatus.Cancelled;
                await _orderRepository.UpdateAsync(order);

                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}
