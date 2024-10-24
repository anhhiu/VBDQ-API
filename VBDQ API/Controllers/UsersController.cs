using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VBDQ_API.Data;
using VBDQ_API.Dtos;
using VBDQ_API.Models;

namespace VBDQ_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbcontext _context;

        public UsersController(MyDbcontext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUser()
        {
            var user =  await _context.Users.ToListAsync();
             return Ok(user);
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDto userDto)
        {
           

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId == id);

            if (user == null)
                return NotFound();

            user.UserName = userDto.UserName;
            user.Password = HashPassWord(userDto.Password);
            user.FullName = userDto.FullName;
            user.Email = userDto.Email;
            user.Phone = userDto.Phone;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        
        [HttpPost("register")]

        public async Task<ActionResult<User>> RegisterUser(Register register)
        {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về mã 400 nếu dữ liệu không hợp lệ
            }

            // Kiểm tra xem tên người dùng đã tồn tại chưa
            if (await _context.Users.AnyAsync(u => u.UserName == register.UserName))
            {
                return Conflict("Tên người dùng đã tồn tại."); // Trả về mã 409 nếu tên người dùng đã tồn tại
            }

            // Tạo một đối tượng User mới từ Register
            var user = new User
            {
                UserName = register.UserName,
                Email = register.Email,
                FullName = register.FullName,
                Password = HashPassword(register.Password),
                Phone = register.Phone,
            };

            // Thêm người dùng vào DbContext
            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // Lưu thay đổi vào cơ sở dữ liệu

            // Trả về kết quả với mã trạng thái 201 (Created)
            return CreatedAtAction("GetUser", new { id = user.UserId }, user); // Trả về đường dẫn đến phương thức GetUser
        }

        // Phương thức để mã hóa mật khẩu (cần phải định nghĩa)
        private string HashPassword(string password)
        {
            // Thực hiện mã hóa mật khẩu ở đây (sử dụng bcrypt, SHA256, v.v.)
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password)); // Chỉ là ví dụ
        }

        private string HashPassWord (string password)
        {
            return Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(password));
        }

        [HttpPost("login")]
        public async Task<ActionResult<User>> LoginUser(Login login)
        {
            // Kiểm tra tính hợp lệ của dữ liệu đầu vào
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Trả về mã 400 nếu dữ liệu không hợp lệ
            }

            // Tìm người dùng theo tên người dùng
            var user = await _context.Users.SingleOrDefaultAsync(u => u.UserName == login.UserName);

            if (user == null)
            {
                return Unauthorized("Tên người dùng không tồn tại."); // Trả về mã 401 nếu không tìm thấy người dùng
            }

            // Kiểm tra mật khẩu (giả định có phương thức VerifyPassword)
            if (!VerifyPassword(user, login.Password))
            {
                return Unauthorized("Mật khẩu không chính xác."); // Trả về mã 401 nếu mật khẩu sai
            }

            // Tạo token hoặc thực hiện bất kỳ thao tác nào cần thiết sau khi đăng nhập thành công
            var token = GenerateToken(user); // Giả định có phương thức GenerateToken

            return Ok(new { User = user, Token = token }); // Trả về thông tin người dùng cùng với token
        }

        // Phương thức để xác thực mật khẩu (cần phải định nghĩa)
        private bool VerifyPassword(User user, string password)
        {
            // Kiểm tra mật khẩu (thực hiện so sánh an toàn)
            // Đây chỉ là ví dụ; bạn nên sử dụng phương thức mã hóa an toàn
            return user.Password == HashPassword(password); // So sánh hash
        }

        // Phương thức tạo token (giả định có phương thức này)
        private string GenerateToken(User user)
        {
            
            return "dangnhapthanhcong";
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
