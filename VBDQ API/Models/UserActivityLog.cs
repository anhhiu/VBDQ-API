using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VBDQ_API.Models
{
    public class UserActivityLog
    {
        [Key]
        public int LogId { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
        public DateTime ActionTime { get; set; }
        [JsonIgnore]
        public User User { get; set; }
    }
}
