using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VBDQ_API.Dtos;
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

        public async Task<IActionResult> GetAllTransaction()
        {
            var (transaction, mes) = await service.GetAllTransaction();

            if (mes.Error == null)
            {
                return Ok(transaction);
            }
            return BadRequest(mes.Status);
        }
        [HttpPost]

        public async Task<IActionResult> AddTransaction(TransactionPP model)
        {
            var (transaction, mes) = await service.AddTransaction(model);

            if (mes.Error == null)
            {
                return Ok(transaction);
            }
            return BadRequest(mes.Status);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetTransactionById(int id)
        {
            var (transaction, mes) = await service.GetTransactionById(id);

            if (mes.Error == null)
            {
                return Ok(transaction);
            }
            return BadRequest(mes.Status);
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> UpdateTranSaction(TransactionPP model, int id)
        {
            var (transaction, mes) = await service.UpdateTransaction(model, id);

            if (mes.Error == null)
            {
                return Ok(transaction);
            }
            return BadRequest(mes.Status);
        }

        [HttpDelete("{id}")]

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
