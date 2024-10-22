using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VBDQ_API.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        
        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public Customer Customer { get; set; }

        public virtual IEnumerable<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }

}
