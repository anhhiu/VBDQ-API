using System.Text.Json.Serialization;

namespace VBDQ_API.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public int CategoryId { get; set; }
        public string? Weight { get; set; }
            
        public int SupplierId { get; set; }
        public string? Description { get; set; }
       
        public int Quantity { get; set; }
        
        public double ProductPrice { get; set; }

        public double Discount { get; set; }

        public bool Available { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
        [JsonIgnore]
        public virtual Supplier? Supplier { get; set; }

        public IEnumerable<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();

    }
      
}
