using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface IProductService
    {
        public Task<(IEnumerable<ProductDto>, Mess)> GetAllProduct();
        public Task<(ProductDto, Mess)> GetProductById();
        public Task<(ProductDto, Mess)> AddProduct(ProductDto productDto);
        public Task<(ProductDto, Mess)> UpdatedProduct(ProductDto productDto, int id);
        public Task<Mess> DeleteProduct(int id);
    }
}
