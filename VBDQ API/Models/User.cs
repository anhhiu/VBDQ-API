using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VBDQ_API.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage ="phai nhap")]
        public string? UserName { get; set; }
        [EmailAddress]
        public string? Email { get; set; } 
        [Required(ErrorMessage = "phai nhap")]
        public string? Password { get; set; }
        public string? FullName { get; set; }
        [Phone]
        public string? Phone { get; set; }
        // xac thuc phan quyen
        public virtual ICollection<UserRole>? UserRoles { get; set; }
        // ghi log  hoat dong nguoi dung
        public virtual ICollection<UserActivityLog>? UserActivityLogs { get; set; }
        
    }

}
