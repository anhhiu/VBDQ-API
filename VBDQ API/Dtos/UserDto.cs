using System.ComponentModel.DataAnnotations;
using VBDQ_API.Models;

namespace VBDQ_API.Dtos
{
    public class UserDto
    {
        [Required(ErrorMessage = "phai nhap")]
        public string? UserName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Required(ErrorMessage = "phai nhap")]
        public string? Password { get; set; }

        [Required(ErrorMessage ="phai nhap")]
        public string? FullName { get; set; }

        [Required(ErrorMessage ="phai nhap")]
        [Phone]
        public string Phone { get; set; } = string.Empty;
      
    }

    public class Register
    {
        [Required(ErrorMessage = "Phải nhập tên người dùng.")]
        public string? UserName { get; set; }

        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Phải nhập mật khẩu.")]
        public string? Password { get; set; }

        [Compare("Password", ErrorMessage = "Mật khẩu không khớp.")]
        [Required(ErrorMessage = "Phải nhập xác nhận mật khẩu.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Phải nhập họ tên.")]
        public string? FullName { get; set; }

        [Required(ErrorMessage = "Phải nhập số điện thoại.")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string? Phone { get; set; }
    }


    public class Login
    {
        [Required(ErrorMessage = "Phải nhập tên người dùng.")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Phải nhập mật khẩu.")]
        public string? Password { get; set; }
    }

}
