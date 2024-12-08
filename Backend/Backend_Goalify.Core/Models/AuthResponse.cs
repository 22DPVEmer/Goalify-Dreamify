namespace Backend_Goalify.Core.Models
{
    public class AuthResponse
    {
    public bool Success { get; set; }
    public string Message { get; set; }         
    public string Token { get; set; }
    public UserModel User { get; set; }
} 
}