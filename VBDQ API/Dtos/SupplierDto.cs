using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class SupplierDto
    {
        public int SupplierId { get; set; }
        public string SupplierName { get; set; }
        public string ContactName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }

        public virtual ICollection<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
    }
}
