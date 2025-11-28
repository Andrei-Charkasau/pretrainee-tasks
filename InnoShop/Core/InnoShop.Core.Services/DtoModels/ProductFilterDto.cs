namespace InnoShop.Core.Services.DtoModels
{
    public class ProductFilterDto
    {
        public string? SearchTerm { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? Availability { get; set; }
        public DateTime? CreatedAfter { get; set; }
        public DateTime? CreatedBefore { get; set; }
        public int? CreatorId { get; set; }
    }
}
