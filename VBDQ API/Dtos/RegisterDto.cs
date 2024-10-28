using System.ComponentModel.DataAnnotations;

namespace VBDQ_API.Dtos
{
    public class RegisterDto
    {
        [Required]
        public string? FullName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? ConfilmPassword { get; set; }

        public string? Role { get; set; }
    }
}
