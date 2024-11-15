using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using VBDQ_API.Conmon;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService service;

        public TransactionController(ITransactionService service)
        {
            this.service = service;
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
        public async Task<IActionResult> GetAllTransaction([FromQuery]GridQuery gridQuery)
        {
           var( response, skip, page, total) = await service.GetTransactionAsync(gridQuery);

            if(response == null)
            {
                return StatusCode(response!.StatusCode);
            }

            return Ok(new
            {
                Data = response,
                Skip = skip,
                Page = page,
                Total = total
            });
        }
     
        [HttpPost("create")]
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
