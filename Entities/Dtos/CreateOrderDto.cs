namespace Ecomm_demo.Entities
{
    public class CreateOrderDto
    {
        public int CustomerId { get; set; }
       
        public virtual ICollection<OrderItem>Items { get; set; }
    }
}
