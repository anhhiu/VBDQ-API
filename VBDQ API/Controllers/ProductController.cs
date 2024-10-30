using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService service;

        public ProductController(IProductService service)
        {
            this.service = service;
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
    }
}
