namespace Ecomm_demo.Entities
{
    public class CreateCategoryDto
    {
     
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual ICollection<Product> Products { get; set; }
    }
}
