using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VBDQ_API.Models
{
    public class UserRole
    {
        [Key]
        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Role Role { get; set; }
    }
}
