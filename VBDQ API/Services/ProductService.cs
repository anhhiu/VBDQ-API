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
            var product = await context.Products
                .OrderBy(x => x.ProductId)
                 .Skip((page-1)*limit)
                 .Take(limit)
                 .ToListAsync();
            int total = await context.Products.CountAsync();

            if (product.Count == 0)
            {
                return (null!,0,0,0, new Mess { Error = string.Empty, Status = "khong co kk" });
            }

            return (product, total, limit, page, new Mess { Error = null!, Status ="sucess"});
                
        }

        //public async Task<(List<ProductDtoName>, Mess)> GetAllProductNamehihi(string? productName, string? categoryName, int skip, int limit)
        //{
        //    var query = context.Products.AsQueryable();

        //    // Nếu productName có giá trị, thêm điều kiện vào truy vấn
        //    if (!string.IsNullOrEmpty(productName))
        //    {
        //        query = query.Where(x => x.ProductName.ToLower().Contains(productName.ToLower()));
        //    }

        //    // Nếu categoryName có giá trị, thêm điều kiện vào truy vấn
        //    if (!string.IsNullOrEmpty(categoryName))
        //    {
        //        query = query.Where(x => x.Category.Name.ToLower().Contains(categoryName.ToLower()));
        //    }


        //    var product = await query.Select(x => new ProductDtoName
        //    {
        //        ProductName = x.ProductName,
        //        CategoryName = x.Category.Name,
        //        Description = x.Description,
        //        Quantity = x.Quantity,
        //        Discount = x.Discount,
        //        Weight = x.Weight

        //    })
        //                                        .Skip(limit * (skip - 1))
        //                                        .Take(limit)
        //                                        .ToListAsync();


        //    return (product, new Mess { Error = null, Status = "ok" });

        //}

        //public async Task<(List<Product>, Mess)> GetAllProductNamehihi(string? productName, string? categoryName, int skip, int limit)
        //{
        //    var query = context.Products.AsQueryable();

        //    // Nếu productName có giá trị, thêm điều kiện vào truy vấn
        //    if (!string.IsNullOrEmpty(productName))
        //    {
        //        query = query.Where(x => x.ProductName.ToLower().Contains(productName.ToLower()));
        //    }

        //    // Nếu categoryName có giá trị, thêm điều kiện vào truy vấn
        //    if (!string.IsNullOrEmpty(categoryName))
        //    {
        //        query = query.Where(x => x.Category.Name.ToLower().Contains(categoryName.ToLower()));
        //    }

        //    // Lấy sản phẩm sau khi đã áp dụng các điều kiện tìm kiếm
        //    var product = await query
        //        .Select(x => new ProductDtoName
        //        {
        //            ProductName = x.ProductName,
        //            CategoryName = x.Category.Name ?? "null",
        //            Description = x.Description,
        //            Quantity = x.Quantity,
        //            Discount = x.Discount,
        //            Weight = x.Weight
        //        })
        //        .Skip(limit * (skip - 1))
        //        .Take(limit)
        //        .ToListAsync();

        //    return (product, new Mess { Error = null, Status = "ok" });
        //}

        public async Task<(List<Product>?, int page, int limit, int total, Mess)> GetALlProductNamePage(string? name, int page, int limit)
        {
            // Kiểm tra nếu 'name' không có giá trị, lấy tất cả sản phẩm
            var query = string.IsNullOrEmpty(name)
                ? context.Products
                : context.Products.Where(x => x.ProductName.ToLower().Contains(name.ToLower()));

            int total = await query.CountAsync();

            var product = await query.OrderBy(x => x.ProductId)
                                     .Skip((page - 1) * limit)
                                     .Take(limit)
                                     .ToListAsync();

            if (product.Count == 0)
            {
                return (null, 0, 0, 0, new Mess { Error = "khong co gi", Status = "khong co gi ca" });
            }
            else
            {
                return (product, total,limit, page  , new Mess { Error = null, Status = "sucess" });
            }
        }


        //public async Task<(IEnumerable<CategoryListProduct>, Mess)> GetCategoriesNameList()
        //{
        //    var category = await context.Categories.GroupJoin(context.Products,
        //        cate => cate.CategoryId, prod => prod.CategoryId
        //        , (cate, prod) => new
        //        {
        //            Cate = cate.Name,
        //            Prod = prod.ToList(),
        //        })
        //        .Select(x => new CategoryListProduct
        //        {
        //            CateName = x.Cate,
        //            products = x.Prod.ToList(),
        //        })
        //        .ToListAsync();

        //    if (category.Count == 0)
        //    {
        //        return (null, new Mess { Error = "khong co", Status = "khong co dau haha" });
        //    }
        //    else
        //    {
        //        return (category, new Mess { Error = null, Status = "sucess" });
        //    }

        //}

        //public async Task<(IEnumerable<CateProDto>, Mess)> GetCatePro()
        //{
        //    var catepro = await context.Products
        //                        .Join(context.Categories,
        //                        product => product.CategoryId,
        //                        categories => categories.CategoryId,
        //                        (product, categories) => new
        //                            {
        //                                CategoryName = categories.Name,
        //                                ProductName = product.ProductName,
        //                                Quantity = product.Quantity,
        //                                Price = product.ProductPrice,
        //                            })
        //                        .Select(x => new CateProDto
        //                            {
        //                                NameCate = x.CategoryName,
        //                                NamePro = x.ProductName,
        //                                Quantity = x.Quantity,
        //                                Price = x.Price,
        //                            })
        //                        .ToListAsync();
        //    if(catepro.Count == 0)
        //    {
        //        return (null, new Mess { Error = "khong co", Status = "khong co dau haha" });
        //    }
        //    else
        //    {
        //        return (catepro, new Mess { Error = null, Status = "sucess" });
        //    }
        //}


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
              

                await context.SaveChangesAsync();

            //    var productClient = mapper.Map<ProductDto>(product);

                return (product, new Mess { Error = null, Status = "update product sucess" });
            }
            catch (Exception ex)
            {
                return (null, new Mess { Error = "loi roi", Status = ex.Message });
            }

        }

        //Task<IEnumerable<Product>> IProductService.GetAllProduct()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<(Product, Mess)> IProductService.AddProduct(ProductDto model)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<IEnumerable<Product>> IProductService.GetAllProduct()
        //{
        //    throw new NotImplementedException();
        //}

        //Task<(Product, Mess)> IProductService.GetProductById(int id)
        //{
        //    throw new NotImplementedException();
        //}

        //Task<(Product, Mess)> IProductService.UpdatedProduct(ProductDto model, int id)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
