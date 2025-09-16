namespace Ecomm_demo.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

        // Computed property
        public string FullName => $"{FirstName} {LastName}";
    }
}
