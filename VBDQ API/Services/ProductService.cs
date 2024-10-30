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
            var product = await context.Products.
                Include(p => p.TransactionDetails)
                .OrderByDescending(p => p.ProductId)
                .ToListAsync();

            var productdto = mapper.Map<IEnumerable<ProductDto>>(product);

            return (productdto, new Mess { Error = null, Status = "sucess" });
        }

        public async Task<(List<Product>, Mess)> GetAllProduct(int skip, int take)
        {
            var product = await context.Products.Include(x  => x.TransactionDetails)
                .OrderBy(x => x.ProductId)
                 .Skip((skip-1)*take)
                 .Take(take)
                 .ToListAsync();

            if (product.Count == 0)
            {
                return (null!, new Mess { Error = string.Empty, Status = "khong co kk" });
            }
            return (product, new Mess { Error = null!, Status ="sucess"});
                
        }

        public async Task<(List<Product>, Mess)> GetALlProductNamePage(string name, int skip, int take)
        {
            var product = await context.Products.Include(x => x.TransactionDetails)
                                                .Where(x => x.ProductName.ToLower().Contains(name.ToLower()))
                                                .OrderBy(x => x.ProductId)
                                                .Skip((skip - 1)*take)
                                                .Take(take)             
                                                .ToListAsync();

            if (product.Count == 0)
            {
                return (null!, new Mess { Error = "khong co gi", Status = "khong co gi ca" });
            }
            else
            {
                return (product, new Mess {Error = null, Status = "sucess" });
            }
                                                
        }

        public async Task<(IEnumerable<CategoryListProduct>, Mess)> GetCategoriesNameList()
        {
            var category = await context.Categories.GroupJoin(context.Products,
                cate => cate.CategoryId, prod => prod.CategoryId
                , (cate, prod) => new
                {
                    Cate = cate.Name,
                    Prod = prod.ToList(),
                })
                .Select(x => new CategoryListProduct
                {
                    CateName = x.Cate,
                    products = x.Prod.ToList(),
                })
                .ToListAsync();

            if (category.Count == 0)
            {
                return (null, new Mess { Error = "khong co", Status = "khong co dau haha" });
            }
            else
            {
                return (category, new Mess { Error = null, Status = "sucess" });
            }

        }

        public async Task<(IEnumerable<CateProDto>, Mess)> GetCatePro()
        {
            var catepro = await context.Products
                                .Join(context.Categories,
                                product => product.CategoryId,
                                categories => categories.CategoryId,
                                (product, categories) => new
                                    {
                                        CategoryName = categories.Name,
                                        ProductName = product.ProductName,
                                        Quantity = product.Quantity,
                                        Price = product.ProductPrice,
                                    })
                                .Select(x => new CateProDto
                                    {
                                        NameCate = x.CategoryName,
                                        NamePro = x.ProductName,
                                        Quantity = x.Quantity,
                                        Price = x.Price,
                                    })
                                .ToListAsync();
            if(catepro.Count == 0)
            {
                return (null, new Mess { Error = "khong co", Status = "khong co dau haha" });
            }
            else
            {
                return (catepro, new Mess { Error = null, Status = "sucess" });
            }
        }

        public async Task<(Product, Mess)> GetProByName(string name)
        {
            var product = await context.Products.
                FirstOrDefaultAsync(p => p.ProductName.ToLower().Contains(name.ToLower()));

            if (product == null)
            {
                return (null!, new Mess { Error = string.Empty, Status = $"khong co san pham nao co name = {name}" });
            }
            else
            {
                return (product, new Mess {Error = null! , Status = "sucess" });
            }
        }

        public async Task<(ProductDto, Mess)> GetProductById(int id)
        {
            try
            {
                var product = await context.Products
                    .Include(p => p.TransactionDetails)
                    .FirstOrDefaultAsync(p => p.ProductId == id);

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

        public async Task<(List<Product>, Mess)> GetProductByName(string name)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                {
                    return (null, new Mess {Error = "loi roi", Status = "muon tim thi phai nhap chu?" });
                }
                var product = await context.Products
                    .Where(p => p.ProductName.ToLower().Contains(name.ToLower()))
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
