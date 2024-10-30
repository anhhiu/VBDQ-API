using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface IProductService
    {
        public Task<(IEnumerable<ProductDto>, Mess)> GetAllProduct();
        public Task<(ProductDto, Mess)> GetProductById(int id);
        public Task<(ProductDto, Mess)> AddProduct(ProductDto productDto);
        public Task<(ProductDto, Mess)> UpdatedProduct(ProductDto productDto, int id);
        public Task<(List<Product>, Mess)> GetProductByName(string name);
        public Task<(List<Product>, Mess)> GetALlProductNamePage(string name, int skip, int take);
        public Task<(List<Product>, Mess)> GetAllProduct(int skip, int take);

        public Task<(Product, Mess)> GetProByName(string name);

        public Task<(IEnumerable<CateProDto>, Mess)> GetCatePro(); 

        public Task<(IEnumerable<CategoryListProduct>, Mess)> GetCategoriesNameList();

        
        public Task<Mess> DeleteProduct(int id);
    }
}
