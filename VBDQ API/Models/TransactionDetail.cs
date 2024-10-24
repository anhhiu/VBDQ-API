using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VBDQ_API.Models
{
    public class TransactionDetail
    {
        [Key]
        public int TransactionDetailId { get; set; }
        public int TransactionId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double UnitPrice { get; set; }
        public double TotalPrice { get; set; }
        [JsonIgnore]
        public Product? Product { get; set; }
        [JsonIgnore]
        public Transaction? Transaction { get; set; }

    }
}
