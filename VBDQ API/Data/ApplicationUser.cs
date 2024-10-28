using Microsoft.AspNetCore.Identity;

namespace VBDQ_API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string? FullName { get; set; }
    }
}
