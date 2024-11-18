namespace VBDQ_API.Dtos
{
    public class ChangePasswordRequet
    {
        public string? UserId { get; set; }
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public class UserDto
    {
        public string? Id { get; set; }             
        public string? UserName { get; set; }     
        public string? Email { get; set; }            
        public string? PhoneNumber { get; set; }    
        public IList<string>? Roles { get; set; }   
    }


    public class ForgotPassword
    {
        public string? Email { get; set; }
    }

    public class ResetPasswordRequet
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
    }
}
