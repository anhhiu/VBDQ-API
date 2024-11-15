using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class TransactionDetailDto
    {
        public int TransactionDetailId { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double Discount { get; set; }
        public double TotalPrice { get; set; }
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ProductDto? ProductDto { get; set; }
        [JsonIgnore]
        public TransactionDto? TransactionDto { get; set; }
    }

    public class TransactionDetailCreate
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        
        [JsonIgnore]
        public Product? Product { get; set; }
        [JsonIgnore]
        public Transaction? Transaction { get; set; }
    }

}
