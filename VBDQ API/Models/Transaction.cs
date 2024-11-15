using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VBDQ_API.Conmon;
using VBDQ_API.Dtos;

namespace VBDQ_API.Models
{
    public class Transaction
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; } = DateTime.Now;
        public double TotalAmount { get; set; }
        public string? PaymentMethod { get; set; } = StatusTransactions.ThanhToanKhiNhanHang;
        [Phone]
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public DateTime UpdatedAt { get; set; }
        // phi van chuyen
        public double ShippingFee { get; set; }
        public  string? Notes { get; set; }
        public string? TransactionStatus { get; set; }
        public string? PaymentStatus { get; set; } 
        [JsonIgnore]
        public Customer? Customer { get; set; }

        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new List<TransactionDetail>();
    }

}
