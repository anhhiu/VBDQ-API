using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using VBDQ_API.Conmon;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Orther;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace VBDQ_API.Services
{
    public class AccountService : IAccountService
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public async Task<ServiceResponse<UserDto>?> ChangePassword(ChangePasswordRequet model)
        {
            var response = new ServiceResponse<UserDto>();

            if(model == null || string.IsNullOrWhiteSpace(model.UserId))
            {
                response.Data = null;
                response.Message = "thong tin khong hop le";
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                return response;
            }

            var user = await userManager.FindByIdAsync(model.UserId!);
            if (user == null)
            {
                response.Data = null;
                response.Message = "khong tim thay nguoi dung";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }

            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword!, model.NewPassword!);
            if (result.Succeeded)
            {
                var roles = await userManager.GetRolesAsync(user);
                var userDto = new UserDto
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    Roles = roles,
                };
                response.Data = userDto;
                response.Message = "Doi mat khau thanh cong";
                response.StatusCode = (int)HttpStatusCode.OK;
                return response;
            }

            response.Data = null;
            response.Message = "khong thanh cong" +  string.Join(",  ",result.Errors.Select(t => t.Description)) ;
            response.StatusCode = (int)HttpStatusCode.BadRequest;
            return response;

        }

        public async Task<ServiceResponse<dynamic>?> DeleteUserById(string id)
        {
            var response = new ServiceResponse<dynamic>();
            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                response.Data = new { };
                response.Message = "khong tim thay";
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }

            await userManager.DeleteAsync(user);
            
            
            response.Data = user;
            response.Message = "da tim thay user co id:" + id;
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;
        }

        public async Task<(IEnumerable<IdentityRole>?, Mess)> GetRoler()
        {
            var role =await roleManager.Roles.ToListAsync();

            if (role == null)
            {
                return (null, new Mess { Error = "khong co gi", Status = "cha co cai deo gi" });
            }
            else
            {
                return (role, new Mess { Error = null, Status = "ok" });
            }
        }

        public async Task<ServiceResponse<dynamic>?> GetUserById(string id)
        {
            var response = new ServiceResponse<dynamic>();
            var user = await userManager.FindByIdAsync(id);

            if(user == null)
            {
                response.Data = new { };
                response.Message = "khhong tim thay id: "+ id;
                response.StatusCode = (int)HttpStatusCode.NotFound;
                return response;
            }
            response.Data = user;
            response.Message = "da thay id : " + id;
            response.StatusCode = (int)HttpStatusCode.OK;
            return response;

        }

        public async Task<(IEnumerable<IdentityUser>?, Mess)> GetUsers()
        {
            var users = await userManager.Users.ToListAsync();

            if (users == null)
            {
                return (null, new Mess { Error = "loi roi", Status = "khong co nguoi dung nao" });
            }
            else
            {
                return (users, new Mess { Error = null, Status = "SUCESS" });
            }
        }

        public async Task<(string, Mess)> Login(LoginDto model)
        {
            var user = await userManager.FindByNameAsync(model.UserName!);
            var passwordValid = await userManager.CheckPasswordAsync(user!, model.Password!);

            if ( user == null || !passwordValid )
            {
                return (string.Empty, new Mess { Error = " loi roi", Status = "thong tin dang nhap khong chinh xac"});
            }

            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName!),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoler = await userManager.GetRolesAsync(user);

            foreach (var role in userRoler)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }
            var skey = configuration["JWT:Secret"];
            var newKeys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(skey!));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.UtcNow.AddMinutes(20),
                claims: authClaim,
                signingCredentials: new SigningCredentials(newKeys, SecurityAlgorithms.HmacSha256Signature)
                );
            var tokenStr = new JwtSecurityTokenHandler().WriteToken(token);
            return (tokenStr, new Mess { Error = null, Status = "Sucess" });
        }

        public async Task<(IdentityResult?, Mess)> Register(RegisterDto model)
        {
            var use = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
            };
    
            var result = await userManager.CreateAsync(use, model.Password!);

            if (result.Succeeded)
            {
                var role = model.Role ?? AppRole.Customer;


                if(! await roleManager.RoleExistsAsync(role)){

                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRoleAsync(use, role);

                return (result, new Mess { Error = null, Status = "tao tai khoan thanh cong" });
            }

            return (null, new Mess { Error = string.Empty, Status = "tao tai khoan khong thanh cong"});
        }

        public async Task<ServiceResponse<dynamic>?> UpdateUserById(UserUpdate model, string id)
        {
            var response = new ServiceResponse<dynamic>();
            var user = await userManager.FindByIdAsync(id);
            if(user == null)
            {
                response.Data = new { };
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = "khong tim thay id" ;
                return response;
            }
            user.FullName = model.FullName;
            user.UserName = model.UserName;
            user.Email = model.Email;
            user.PhoneNumber = model.PhoneNumber;
            
            await userManager.UpdateAsync(user);
            response.Data = user;
            response.StatusCode = (int)HttpStatusCode.OK;
            response.Message = "da tim thay id va sua thong tin thanh cong";
            return response;
        }
    }
}
