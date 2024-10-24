namespace VBDQ_API.Dtos
{
    public class CustomerDto
    {
        public int CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public IEnumerable<TransactionDto> TransactionDtos { get; set; } = new List<TransactionDto>();
    }

    public class CustomerPP
    {
       
        public string? CustomerName { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
    }
}
