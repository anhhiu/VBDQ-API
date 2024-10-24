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
            var (supplier, mes) = await service.GetAllSupplier();

           if (mes.Error == null) return Ok(supplier);

           return BadRequest(mes.Status);
        }

        [HttpPost]

        public async Task<IActionResult> AddSupplier(SupplierDto supplierDto)
        {
            var (supplier, mes) = await service.AddSupplier(supplierDto);

            if (mes.Error == null) return Ok(supplierDto);

            return BadRequest(mes.Status);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSupplierById(int id)
        {
            var (supplier, mes) = await service.GetSupplierById(id);

            if (mes.Error == null) return Ok(supplier);

            return BadRequest(mes.Status);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateSupplier(SupplierDto supplierDto, int id)
        {
            var (supplier, mes) = await service.UpdatedSupplier(supplierDto, id);

            if (mes.Error == null) return Ok(supplierDto);

            return BadRequest(mes.Status);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var mes = await service.DeleteSupplier(id);

            if (mes.Error == null) return Ok(mes.Status);
            return BadRequest(mes.Status);
        }

    }
}
