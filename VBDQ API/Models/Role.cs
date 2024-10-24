using System.ComponentModel.DataAnnotations;

namespace VBDQ_API.Models
{
    public class Role
    {
        [Key]
        public int RoleId { get; set; }
        public string? RoleName { get; set; }
        public virtual ICollection<UserRole>? UserRoles { get; set; }
    }
}
