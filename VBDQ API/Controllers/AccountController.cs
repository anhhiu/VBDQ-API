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

        [HttpGet("users")]

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
        [HttpGet("roles")]
        public async Task<IActionResult> GetRoles()
        {
            var (roles, mes) = await service.GetRoler();

            if (mes.Error == null)
            {
                return Ok(roles);
            }
            else
            {
                return StatusCode(500, mes.Status);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var response = await service.GetUserById(id);
            if(response != null)
            {
                return StatusCode(response.StatusCode, response);
            }
            return StatusCode(response!.StatusCode, response.Message);

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            var response = await service.DeleteUserById(id);
            if (response != null)
            {
                return StatusCode(response.StatusCode, response);
            }
            return StatusCode(response!.StatusCode, response.Message);

        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserById(UserUpdate model, string id)
        {
            var response = await service.UpdateUserById(model, id);
            if (response != null)
            {
                return StatusCode(response.StatusCode, response);
            }
            return StatusCode(response!.StatusCode, response.Message);

        }

        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordRequet model)
        {
            var response = await service.ChangePassword(model);
            if (response != null)
            {
                return StatusCode(response.StatusCode, response);
            }
            return StatusCode(response!.StatusCode, response.Message);
        }
    }
}

