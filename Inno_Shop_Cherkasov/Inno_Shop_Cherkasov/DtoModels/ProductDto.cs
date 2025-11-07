namespace Inno_Shop_Cherkasov.DtoModels
{
    public class ProductDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public int CreatorId { get; set; }
    }
}
