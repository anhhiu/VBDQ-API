using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class CategoryDto
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<ProductDto> ProductDtos { get; set; } = new List<ProductDto>();
    }

    public class CategoryCreate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }

    public class CategoryUpdate
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
    }
}
