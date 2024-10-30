using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionDetailController : ControllerBase
    {
        private readonly ITransactionDetailService service;

        public TransactionDetailController(ITransactionDetailService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var (trans, mes) = await service.GetAllTransactionDetail();
            if(mes.Error != null)
            {
                return BadRequest(mes.Status);
            }
            return Ok(trans);

        }
    }
}
