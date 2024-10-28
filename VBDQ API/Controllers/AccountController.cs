using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using VBDQ_API.Dtos;
using VBDQ_API.Services;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService service;

        public AccountController(IAccountService service)
        {
            this.service = service;
        }

        [HttpPost("login")]

        public async Task<IActionResult> Login(LoginDto model)
        {
            var (token, mes) = await service.Login(model);

            if (mes.Error == null)
            {
                return Ok(token);
            }
            else
            {
                return StatusCode(500, mes.Status);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            var (ok, mes) = await service.Register(model);

            if (mes.Error == null)
            {
                return Ok(mes.Status);
            }
            else
            {
                return StatusCode(500, mes.Status);
            }
        }

        [HttpGet]

        public async Task<IActionResult> GetAllUsers()
        {
            var (users, mes) = await service.GetUsers();

            if(mes.Error == null)
            {
                return Ok(users);
            }
            else
            {
                return StatusCode(500, mes.Status);
            }

        }
    }
}

