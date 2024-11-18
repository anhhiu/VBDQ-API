using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using VBDQ_API.Conmon;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService service;
        private readonly MyDbcontext _context;

        public TransactionController(ITransactionService service, MyDbcontext context)
        {
            this.service = service;
            _context = context;
        }

        [HttpGet]
        //[Authorize]
        //[Authorize(Roles = $"{AppRole.Admin}, {AppRole.Customer}")]
        public async Task<IActionResult> GetAllTransaction()
        {
            var (transaction, mes) = await service.GetAllTransaction();

            if (mes.Error == null)
            {
                return Ok(transaction);
            }
            return BadRequest(mes.Status);
        }

        [HttpGet("Gridquery")]
        public async Task<IActionResult> GetAllTransaction([FromQuery] GridQuery gridQuery)
        {
            var (response, skip, page, total) = await service.GetTransactionAsync(gridQuery);

            if (response == null)
            {
                return StatusCode(response.StatusCode);
            }

            return Ok(new
            {
                Data = response,
                Skip = skip,
                Page = page,
                Total = total
            });
        }

        [HttpGet("Projection")]

        public async Task<IActionResult> GetTran()
        {
            var response = await _context.Transactions.AsNoTracking().Select(t => new
            {
                t.TransactionId,
                t.TransactionDate,
                t.PaymentStatus,
                t.PhoneNumber,
                CustomerName = t.Customer != null ? t.Customer.CustomerName : null,
                TransactionDetails =  t.TransactionDetails.Select(d => new
                {
                    d.ProductId,
                    d.Discount,
                    d.UnitPrice,
                    d.Quantity
                }).ToList()
            }).ToListAsync();

            if(response == null  || response.Count == 0)
            {
                return NotFound("loi ");
            }

            return Ok( response);
        }


        [HttpPost]
        public async Task<IActionResult> AddTransactionAsync(TransactionCreate model)
        {
            var transactionResponse = await service.AddTransactionAsync(model);
            if (transactionResponse == null)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, "Lỗi hệ thống");
            }
            return StatusCode(transactionResponse.StatusCode, transactionResponse);
        }


        [HttpGet("{id:int}")]
        //[Authorize(Roles = AppRole.Customer)]
        public async Task<IActionResult> GetTransactionById(int id)
        {
            var (transaction, mes) = await service.GetTransactionById(id);

            if (mes.Error == null)
            {
                return Ok(transaction);
            }
            return BadRequest(mes.Status);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateTransactionAsync(TransactionUpdate model, int id)
        {
            var response = new ServiceResponse<dynamic>();
            response = await service.UpdateTransactionAsync(model, id);

            return StatusCode(response!.StatusCode, response);
        }


        [HttpDelete("{id:int}")]
        //[Authorize(Roles = AppRole.Admin)]
        public async Task<IActionResult> DeleteTranSaction(int id)
        {
            var mes = await service.DeleteTransaction(id);

            if (mes.Error == null)
            {
                return Ok(mes.Status);
            }
            return BadRequest(mes.Status);
        }

    }
}
