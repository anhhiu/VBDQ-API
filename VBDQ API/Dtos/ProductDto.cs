
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class ProductDto
    {
        
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
        public CategoryDto? CategoryDto { get; set; }
        [JsonIgnore]
        public SupplierDto? SupplierDto { get; set; }


    }

    public class ProductDtoName
    {
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? Weight { get; set; }

        public string? Suppliername { get; set; }
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public double ProductPrice { get; set; }

        public double Discount { get; set; }

        public bool Available { get; set; }
        [JsonIgnore]
        public Category? Category { get; set; }
        [JsonIgnore]
        public Supplier? Supplier { get; set; }

     //   public IEnumerable<TransactionDetailDto> TransactionDetailDtos { get; set; } = Enumerable.Empty<TransactionDetailDto>();
    }

    public class ProductCS
    {
        public int Id { get; set; }
        public string? ProductName { get; set; }
        public string? CategoryName { get; set; }
        public string? SupplierName { get; set; }

        public string? ContactName { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public string? Adress { get; set; }
        
    }

    public class ProductCreate
    {
        public string? ProductName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string? Weight { get; set; }
        [Required]
        public int SupplierId { get; set; }
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public double ProductPrice { get; set; }

        public double Discount { get; set; }

        public bool Available { get; set; }
       
    }

    public class ProductUpdate
    {
        public string? ProductName { get; set; }
       
        public string? Weight { get; set; }
        
        public string? Description { get; set; }

        public int Quantity { get; set; }

        public double ProductPrice { get; set; }

        public double Discount { get; set; }

        public bool Available { get; set; }

    }


}
