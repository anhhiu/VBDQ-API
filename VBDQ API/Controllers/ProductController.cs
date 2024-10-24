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
    }
}
