namespace Ecomm_demo.Entities
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public bool IsActive { get; set; } = true;
        public string CategoryName { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
