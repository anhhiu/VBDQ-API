using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;
        private readonly MyDbcontext context;

        public ProductController(IProductService service, MyDbcontext context)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {
            var (product, mes) = await service.GetAllProduct();

            return Ok(product);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var (product, mes) = await service.GetProductById(id);

            if (mes.Error == null) return Ok(product);

            return NotFound(mes.Status);
        }

        [HttpPost]

        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var (product, mes) = await service.AddProduct(productDto);

            if (mes.Error ==null ) return Ok(product);

            return BadRequest(mes.Status);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateProduct(ProductDto productDto, int id)
        {
            var (product, mes) = await service.UpdatedProduct(productDto, id);

            if (mes.Error == null) return Ok(product);

            return BadRequest(mes.Status);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var mes = await service.DeleteProduct(id);

            if (mes.Error == null) return Ok(mes.Status);

            return BadRequest(mes.Status);
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchName([FromQuery] string name)
        {
            var (search, mes) =await service.GetProductByName(name);

            if (mes.Error == null) 
                return Ok(search);

            return BadRequest(mes.Status);
        }

        [HttpGet("pagination")]

        public async Task<IActionResult> GetAllProduct(int skip, int take)
        {
            var (products, mes) = await service.GetAllProduct(skip, take);

            if(mes.Error == null)
                return Ok(products);

            return BadRequest(mes.Status);
        }


        [HttpGet("search-pagination")]

        public async Task<IActionResult> GetAllProuduct(string name, int skip, int take)
        {
            var (products, mes) = await service.GetALlProductNamePage(name, skip, take);

            if (mes.Error == null)
            {
                return Ok(products);
            }
            else
            {
                return BadRequest(mes.Status);
            }
        }

        [HttpGet("search-name")]

        public async Task<IActionResult> GetProductByName([FromQuery] string name)
        {
            var (product, mes) = await service.GetProByName(name);
            if(mes.Error == null)
            {
                return Ok(product);
            }
            else
            {
                return BadRequest(mes.Status);
            }

        }
        [HttpGet("category-product-name")]
        public async Task<IActionResult> GetCatePro()
        {
            var (catepro, mes) = await service.GetCatePro();

            if(mes.Error == null)
            {
                return Ok(catepro);
            }
            else
            {
                return BadRequest(mes.Status);
            }
        }

        [HttpGet("category-product-nameGroup")]
        public async Task<IActionResult> GetCateProNameGroup()
        {
            var (catepro, mes) = await service.GetCategoriesNameList();

            if (mes.Error == null)
            {
                return Ok(catepro);
            }
            else
            {
                return BadRequest(mes.Status);
            }
        }

        [HttpGet("Categoryproductname")]
        public async Task<IActionResult> GetCatePro1()
        {
            try
            {
                var capo = await context.Products.AsNoTracking().Select(x => new CateProDto
                {
                    NameCate = x.Category.Name,
                    NamePro = x.ProductName,
                    Quantity = x.Quantity,
                    Price = x.ProductPrice,
                }).OrderByDescending(x => x.Price)
                   .ToListAsync();
                return Ok(capo);
            }catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("ASCending-price")]
        public async Task<IActionResult> AscendingPrice()
        {
            var asc = await context.Products.OrderBy(x => x.ProductPrice).ToListAsync();

            return Ok(asc);
        }

        [HttpGet("Descending-price")]
        public async Task<IActionResult> DescendingPrice()
        {
            var asc = await context.Products.OrderByDescending(x => x.ProductPrice).ToListAsync();

            return Ok(asc);
        }

        [HttpGet("Max-price")]
        public async Task<IActionResult> MaxPrice()
        {
            var max = await context.Products.MaxAsync(x => x.ProductPrice);

            var min = await context.Products.MinAsync(x => x.ProductPrice);

            var pro = await context.Products.CountAsync();


            return Ok(pro);
        }
    }
}
