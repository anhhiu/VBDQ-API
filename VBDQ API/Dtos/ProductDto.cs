using System.Text.Json.Serialization;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string Weight { get; set; }

        public int SupplierId { get; set; }
        public string Description { get; set; }

        public int Quantity { get; set; }

        public double ProductPrice { get; set; }

        public double Discount { get; set; }

        public bool Available { get; set; }
        [JsonIgnore]
        public virtual CategoryDto CategoryDto { get; set; }
        [JsonIgnore]
        public virtual SupplierDto SupplierDto { get; set; }

        public IEnumerable<TransactionDetailDto> TransactionDetailDtos { get; set; } = Enumerable.Empty<TransactionDetailDto>();
    }
}
