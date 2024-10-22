using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class ProductService : IProductService
    {
        public Task<(ProductDto, Mess)> AddProduct(ProductDto productDto)
        {
            throw new NotImplementedException();
        }

        public Task<Mess> DeleteProduct(int id)
        {
            throw new NotImplementedException();
        }

        public Task<(IEnumerable<ProductDto>, Mess)> GetAllProduct()
        {
            throw new NotImplementedException();
        }

        public Task<(ProductDto, Mess)> GetProductById()
        {
            throw new NotImplementedException();
        }

        public Task<(ProductDto, Mess)> UpdatedProduct(ProductDto productDto, int id)
        {
            throw new NotImplementedException();
        }
    }
}
