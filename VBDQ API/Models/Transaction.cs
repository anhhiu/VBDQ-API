using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VBDQ_API.Dtos;

namespace VBDQ_API.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public double TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? TransactionStatus { get; set; } = "Đã mua";
        public string? PaymentStatus { get; set; } = "Đã chuyển";
        [JsonIgnore]
        public Customer? Customer { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }

}
