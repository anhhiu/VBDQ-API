using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;
        public double TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public int Quantity { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public double Discount { get; set; }

        public string? TransactionStatus { get; set; } = "Đã mua";
        public string? PaymentStatus { get; set; } = "Đã Thanh Toán";
        [JsonIgnore]
        public Customer? Customer { get; set; }

        public virtual ICollection<TransactionDetailDto> TransactionDetails { get; set; } = new List<TransactionDetailDto>();
    }

    public class TransactionPP
    {
        public int CustomerId { get; set; }
        public double TotalAmount { get; set; }
        public string? PaymentMethod { get; set; }
        public int Quantity { get; set; }
        [Phone]
        public string? PhoneNumber { get; set; }

        public string? Address { get; set; }

        public double Discount { get; set; }
    }

}
