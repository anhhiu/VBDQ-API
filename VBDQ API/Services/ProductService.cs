using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

        public async Task<(Product?, Mess)> AddProduct(ProductDto productDto)
        {
            try
            {
                var product = new Product
                {
                    ProductName = productDto.ProductName,
                    CategoryId = productDto.CategoryId,
                    Weight = productDto.Weight,
                    SupplierId = productDto.SupplierId,
                    Description = productDto.Description,
                    Quantity = productDto.Quantity,
                    ProductPrice = productDto.ProductPrice,
                    Discount = productDto.Discount,
                    Available = productDto.Available
                };

                await context.Products.AddAsync(product);

                await context.SaveChangesAsync();
                return (product, new Mess { Error = null, Status = "add product sucess" });

            } catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message});
            }
        }

        public async Task<Mess> DeleteProduct(int id)
        {
            try
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

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

        public async Task<IEnumerable<Product>?> GetAllProduct()
        {
            var product = await context.Products
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();
            return (product);
        }

        public async Task<(List<Product>?, int page, int limit, int total, Mess)> GetAllProduct(int page, int limit)
        {
            var query = context.Products.OrderByDescending(p => p.ProductId).AsQueryable();

            
            if(page > 0 && limit > 0)
            {
                query = query.Skip((page -1)*limit).Take(limit);
            }

            var product = await query.ToListAsync();
            int total = await context.Products.CountAsync();

            if (product.Count == 0)
            {
                return (null!,total,0,0, new Mess { Error = string.Empty, Status = "khong co kk" });
            }
            return (product, total, limit, page, new Mess { Error = null!, Status ="sucess"});
                
        }

        public async Task<(List<Product>?, int page, int limit, int total, Mess)> GetALlProductNamePage(string? name, int page, int limit)
        {
            // Kiểm tra nếu 'name' không có giá trị, lấy tất cả sản phẩm
            var query = string.IsNullOrEmpty(name)
                ? context.Products
                : context.Products.Where(x => x.ProductName.ToLower().Contains(name.ToLower()));

            int total = await query.CountAsync();

            if(page > 0 && limit > 0)
            {
                query = query.Skip((page - 1) * limit).Take(limit);
            }

            var product = await query.ToListAsync();

            if (product.Count == 0)
            {
                return (null, total, limit, page, new Mess { Error = "khong co gi", Status = "khong co gi ca" });
            }
            else
            {
                return (product, total,limit, page  , new Mess { Error = null, Status = "sucess" });
            }
        }
        public async Task<(Product?, Mess)> GetProductById(int id)
        {
            try
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                    return (null, new Mess { Error = "loi roi", Status = $"Không tìm thấy sản phẩm có Id = {id}" });
               // var productDto = mapper.Map<ProductDto>(product);

                return (product, new Mess { Error = null, Status = "sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(List<Product>?, Mess)> GetProductByName(string? name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return (null, new Mess {Error = "loi roi", Status = "muon tim thi phai nhap chu?" });
                }
                var product = await context.Products
                    .Where(p => p.ProductName!.ToLower().Contains(name.ToLower()))
                    .ToListAsync();

                if (product.Count == 0)
                {
                    return (null, new Mess { Error = "khong thays", Status = "khong tim thay" });
                }
                else
                {
                   // var productSearch = mapper.Map<ProductDto>(product);
                    return (product, new Mess {Error =null, Status = "sucess" });
                }

            }
            catch (Exception ex)
            {
                return (null, new Mess {Error = "loi roi", Status = ex.Message });
            }
        }

        public async Task<(Product?, Mess)> UpdatedProduct(ProductDto productDto, int id)
        {
            try
            {
                var product = await context.Products.FindAsync(id);

                if (product == null)
                {
                    return (null, new Mess { Error = "loi roi", Status = $"Không tìm thấy sản phẩm có Id = {id}" });
                }
                   

                //mapper.Map(productDto, product);
                product.ProductName = productDto.ProductName;
                product.ProductPrice = productDto.ProductPrice;
                product.Discount = productDto.Discount;
                product.Quantity = productDto.Quantity;
                product.Available = productDto.Available;
              

                await context.SaveChangesAsync();

            //    var productClient = mapper.Map<ProductDto>(product);

                return (product, new Mess { Error = null, Status = "update product sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }

        }
    }
}
