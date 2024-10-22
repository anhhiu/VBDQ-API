using System.ComponentModel.DataAnnotations;

namespace VBDQ_API.Models
{
    public class Customer
    {
        [Key] 
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public IEnumerable<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
