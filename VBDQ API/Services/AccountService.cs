using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public async Task<(IEnumerable<IdentityRole>, Mess)> GetRoler()
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

        public async Task<(IEnumerable<IdentityUser>, Mess)> GetUsers()
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
            var user = await userManager.FindByNameAsync(model.UserName);
            var passwordValid = await userManager.CheckPasswordAsync(user, model.Password);

            if ( user == null || !passwordValid )
            {
                return (string.Empty, new Mess { Error = " loi roi", Status = "khong co chuoi token dau ma haha"});
            }

            var authClaim = new List<Claim>
            {
                new Claim(ClaimTypes.Name, model.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoler = await userManager.GetRolesAsync(user);

            foreach (var role in userRoler)
            {
                authClaim.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var newKeys = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

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

        public async Task<(IdentityResult, Mess)> Register(RegisterDto model)
        {
            var use = new ApplicationUser
            {
                UserName = model.UserName,
                Email = model.Email,
                FullName = model.FullName,
            };
    
            var result = await userManager.CreateAsync(use, model.Password);

            if (result.Succeeded)
            {
                var role = model.Role ?? AppRole.Customer;


                if(! await roleManager.RoleExistsAsync(role)){

                    await roleManager.CreateAsync(new IdentityRole(role));
                }

                await userManager.AddToRoleAsync(use, role);
            }

            return (result, new Mess { Error = null, Status = "Sucess"});
        }
    }
}
