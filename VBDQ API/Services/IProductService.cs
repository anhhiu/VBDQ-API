using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>?> GetAllProduct();
        public Task<(List<Product>?, int page, int limit, int total, Mess)> GetAllProduct(int page, int limit);
        public Task<(Product?, Mess)> AddProduct(ProductDto model);
        public Task<(Product?, Mess)> UpdatedProduct(ProductDto model, int id);
        public Task<(Product?, Mess)> GetProductById(int id);
        public Task<(List<Product>?, Mess)> GetProductByName(string name);
        public Task<(List<Product>?, int page, int limit, int total, Mess)> GetALlProductNamePage(string? name, int page, int limit);
        public Task<Mess> DeleteProduct(int id);

    }
}
