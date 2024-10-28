using Microsoft.AspNetCore.Identity;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface IAccountService
    {
        public Task<(IdentityResult, Mess)> Register(RegisterDto model);
        public Task<(string, Mess)> Login(LoginDto model);

        public Task<(IEnumerable<IdentityUser>, Mess)> GetUsers();
    }
}
