using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class TopProductDto
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int TotalSold { get; set; }
      
        public double Revenue { get; set; }

    }

    public class CateProDto
    {
        public string? NameCate { get; set; }
        public string? NamePro { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

    }

    public class CategoryListProduct
    {
        public string? CateName { get; set; }
        public IEnumerable<Product> products { get; set; } = new List<Product>();
    }
}
