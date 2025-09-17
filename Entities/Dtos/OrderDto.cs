namespace Ecomm_demo.Entities
{
    public class OrderDto
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; } 
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; }
        public string CustomerName { get; set; } = null!;
        public List<OrderItemDto> Items { get; set; }
    }
}
