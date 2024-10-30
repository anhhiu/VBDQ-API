using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;
using VBDQ_API.Orther;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService service;
        private readonly MyDbcontext context;

        public CustomerController(ICustomerService service, MyDbcontext context)
        {
            this.service = service;
            this.context = context;
        }
        [HttpGet("sql-raw")]

        public async Task<IActionResult> GetCus()
        {
            var sql = $@"select * from Customers";

            var cus = await context.Set<Customer>().FromSqlRaw(sql).ToListAsync();

            return Ok(cus);
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

        [Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> AddCustomer([FromForm]CustomerPP customerDto)
        {
            var (customer, mes) = await service.AddCustomer(customerDto);

            if( mes.Error == null)
                return Ok(customer);

            return BadRequest(mes.Status);
        }
        [HttpPut("{id}")]
       
        [Authorize(Roles = AppRole.Admin)]
       
        public async Task<IActionResult> UpdateCustomer([FromForm]CustomerPP customerDto, int id)
        {
            var (customer, mes) = await service.UpdateCustomer(customerDto, id);

            if (mes.Error == null)
                return Ok(customer);

            return BadRequest(mes.Status);
        }

        [HttpDelete("{id}")]
        
        [Authorize(Roles = AppRole.Admin)]
        
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

        [HttpGet("leftjoin")]

        public async Task<IActionResult> GetLiftJoin()
        {
            var lj = await context.Customers
                .GroupJoin(context.Transactions, cus => cus.CustomerId, trans => trans.CustomerId, (cus, trans) => new
                {
                    customerName = cus.CustomerName,
                    Transaction = trans.DefaultIfEmpty(),
                })
                .SelectMany(x => x.Transaction.Select(trans => new
                {
                    Customer = x.customerName,
                    Phone = trans.PhoneNumber,
                    Address = trans.Address,
                    PayMent = trans != null ? trans.PaymentMethod : "quyen me mat chua dien" ,
                })).ToListAsync();

            return Ok(lj);
        }

        [HttpGet("customer")]

        public async Task<IActionResult> GetCustomer1()
        {
            var customerPPs = await context.Customers.Select(x => new CustomerPP
            {
                CustomerName = x.CustomerName,
                Phone = x.Phone,
                Address = x.Address,
            }).ToListAsync();

            return Ok(customerPPs);
        }

    }
}
