using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService service;

        public CustomerController(ICustomerService service)
        {
            this.service = service;
        }

        [HttpGet]

        public async Task<IActionResult> GetAllCustomer()
        {
            var (customer,mes) = await service.GetAlLCustomer();

            if( mes.Error == null)
                return Ok(customer);

            return BadRequest(mes.Status);
            
        }

        [HttpPost]
        
        public async Task<IActionResult> AddCustomer(CustomerPP customerDto)
        {
            var (customer, mes) = await service.AddCustomer(customerDto);

            if( mes.Error == null)
                return Ok(customer);

            return BadRequest(mes.Status);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(CustomerPP customerDto, int id)
        {
            var (customer, mes) = await service.UpdateCustomer(customerDto, id);

            if (mes.Error == null)
                return Ok(customer);

            return BadRequest(mes.Status);
        }

        [HttpDelete("{id}")]

        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var mes = await service.DeleteCustomer(id);

            if( mes.Error == null)
                return Ok(mes.Status);
            return BadRequest(mes.Status);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetCustomerById(int id)
        {
            var (customer, mes) = await service.GetCustomerById(id);

            if (mes.Error == null)
                return Ok(customer);
            return BadRequest(mes.Status);
        }
    }
}
