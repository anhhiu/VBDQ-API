using Microsoft.AspNetCore.Identity;
using VBDQ_API.Conmon;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;

namespace VBDQ_API.Services
{
    public interface IAccountService
    {
        public Task<(IdentityResult?, Mess)> Register(RegisterDto model);
        public Task<(string, Mess)> Login(LoginDto model);
        public Task<(IEnumerable<IdentityRole>?, Mess)> GetRoler();
        public Task<(IEnumerable<IdentityUser>?, Mess)> GetUsers();

        Task<ServiceResponse<dynamic>?> GetUserById(string id);
        Task<ServiceResponse<dynamic>?> DeleteUserById(string id);

        Task<ServiceResponse<dynamic>?> UpdateUserById(UserUpdate model, string id);

        Task<ServiceResponse<UserDto>?> ChangePassword(ChangePasswordRequet model);
        //<ServiceResponse<dynamic>?> ForgotPassword(ForgotPassword model, string id);
    }
}
