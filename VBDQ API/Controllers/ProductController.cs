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
            var product = await service.GetAllProduct();

            return Ok(product);
        }

        [HttpGet("{id:int}")]
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

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateProduct(ProductDto productDto, int id)
        {
            var (product, mes) = await service.UpdatedProduct(productDto, id);

            if (mes.Error == null) return Ok(product);

            return BadRequest(mes.Status);
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteProduct(int id)
        {
            var mes = await service.DeleteProduct(id);

            if (mes.Error == null) return Ok(mes.Status);

            return BadRequest(mes.Status);
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> SearchName(string name)
        {
            var (search, mes) = await service.GetProductByName(name);

            if (mes.Error == null)
                return Ok(search);

            return BadRequest(mes.Status);
        }

        [HttpGet("pagination")]

        public async Task<IActionResult> GetAllProduct(int page, int limit)
        {
            var (products, total,repage,  relimit, mes) = await service.GetAllProduct(page, limit);

            if (mes.Error == null)
                return Ok(new
                {
                    Products = products,
                    Total = total,
                    Limit = relimit,
                    Page = repage,
                });

            return BadRequest(mes.Status);
        }


        [HttpGet("search-pagination")]
        public async Task<IActionResult> GetAllProduct(string? name, int page, int limit)
        {
            var (products, total, returnedPage, returnedLimit, mes) = await service.GetALlProductNamePage(name, page, limit);

            if (mes.Error == null)
            {
                return Ok(new
                {
                    Products = products,
                    Page = returnedPage,
                    Limit = returnedLimit,
                    Total = total,
                    Status = mes.Status
                });
            }
            else
            {
                return BadRequest(new
                {
                    Error = mes.Error,
                    Status = mes.Status
                });
            }
        }
    }
}
