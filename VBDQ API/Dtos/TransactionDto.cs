using System.Text.Json.Serialization;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class TransactionDto
    {
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public DateTime TransactionDate { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentMethod { get; set; }

        public string PhoneNumber { get; set; }
        [JsonIgnore]
        public CustomerDto CustomerDto { get; set; }

        public virtual IEnumerable<TransactionDetailDto> TransactionDetailDtos { get; set; } = new List<TransactionDetailDto>();
    }
}
