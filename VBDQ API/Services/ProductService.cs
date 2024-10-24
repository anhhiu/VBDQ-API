using AutoMapper;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public class ProductService : IProductService
    {
        private readonly MyDbcontext context;
        private readonly IMapper mapper;

        public ProductService(MyDbcontext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public async Task<(ProductDto, Mess)> AddProduct(ProductDto productDto)
        {
            try
            {
                var product = mapper.Map<Product>(productDto);

                await context.Products.AddAsync(product);

                await context.SaveChangesAsync();

                var productClient = mapper.Map<ProductDto>(product);

                return (productClient, new Mess { Error = null, Status = "add product sucess" });
            } catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message});
            }
        }

        public async Task<Mess> DeleteProduct(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);

                if (product == null) return new Mess { Error = "loi roi", Status = $"Không tìm thấy sản phẩm có Id = {id}" };

                context.Products.Remove(product);

                await context.SaveChangesAsync();

                return new Mess { Error = null, Status = $"Xóa thành công sản phẩm có Id = {id}" };

            }
            catch (Exception ex)
            {
                return new Mess { Error = "loi roi", Status = ex.Message };
            }



        }

        public async Task<(IEnumerable<ProductDto>, Mess)> GetAllProduct()
        {
            var product = await context.Products.OrderByDescending(p => p.ProductId).ToListAsync();

            var productdto = mapper.Map<IEnumerable<ProductDto>>(product);

            return (productdto, new Mess { Error = null, Status = "sucess" });
        }

        public async Task<(ProductDto, Mess)> GetProductById(int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);

                if (product == null)
                    return (null, new Mess { Error = "loi roi", Status = $"Không tìm thấy sản phẩm có Id = {id}" });
                var productDto = mapper.Map<ProductDto>(product);

                return (productDto, new Mess { Error = null, Status = "sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(ProductDto, Mess)> UpdatedProduct(ProductDto productDto, int id)
        {
            try
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                    return (null, new Mess { Error = "loi roi", Status = $"Không tìm thấy sản phẩm có Id = {id}" });

                mapper.Map(productDto, product);

                await context.SaveChangesAsync();

                var productClient = mapper.Map<ProductDto>(product);

                return (productClient, new Mess { Error = null, Status = "update product sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }

        }
    }
}
