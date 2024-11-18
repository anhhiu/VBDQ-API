using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService service;
        public SupplierController(ISupplierService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllSupplier()
        {
            var response = await service.GetAllSupplierAsync();

           if (response != null) return Ok(response);

           return StatusCode(response.StatusCode, response);
        }

        [HttpGet("page")]
        public async Task<IActionResult> GetAllSupplier(int PageNumber, int PageSize)
        {
            var response = await service.GetAllSupplierAsync2(PageNumber , PageSize);

            if (response != null) return Ok(response);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPost]

        public async Task<IActionResult> AddSupplier(SuppllierCreate model)
        {
            var response = await service.CreateSupplierAsync(model);

            if (response != null) return Ok(response);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var response = await service.GetSupplierByIdAsync(id);

            if (response != null) return Ok(response);

            return StatusCode(response.StatusCode, response);
        }

        [HttpPut("{id:int}")]

        public async Task<IActionResult> UpdateSupplier(SuppllierUpdate model, int id)
        {
            var response = await service.UpdateSupplierAsync(model, id);

            if (response != null) return Ok(response);

            return StatusCode(response.StatusCode, response);
        }

        [HttpDelete("{id:int}")]

        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var response = await service.DeleteSupplierByIdAsync(id);

            if (response != null) return Ok(response);

            return StatusCode(response.StatusCode, response);
        }

    }
}
