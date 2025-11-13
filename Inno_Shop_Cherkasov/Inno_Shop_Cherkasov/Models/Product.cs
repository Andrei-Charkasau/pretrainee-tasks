namespace Inno_Shop_Cherkasov.Models
{
    public class Product : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Availability { get; set; }
        public int CreatorId { get; set; }

        public Product() { }
    }
}
