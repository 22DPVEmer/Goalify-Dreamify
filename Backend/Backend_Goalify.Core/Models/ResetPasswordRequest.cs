namespace Backend_Goalify.Core.Models
{
    public class ResetPasswordRequest
    {
        public string Email { get; set; }      // The user's email address
        public string Token { get; set; }      // The token for verification
        public string NewPassword { get; set; } // The new password to set
    }
}