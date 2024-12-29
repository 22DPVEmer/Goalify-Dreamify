using Backend_Goalify.Core.Entities;
using System.Threading.Tasks;
using Backend_Goalify.Core.Models;

public interface IAuthService
{
    Task<(bool success, string token, ApplicationUser user)> LoginAsync(LoginModel model);
    Task<(bool success, string message)> RegisterAsync(RegisterModel model);
    Task<Result> AssignRoleAsync(string userId, string role);
    Task<Result> RefreshTokenAsync(string token);
    Task<(bool success, string token, string message)> GeneratePasswordResetTokenAsync(string email);
    Task<(bool success, string message)> ResetPasswordAsync(ResetPasswordRequest request);
    Task<(bool success, string message)> VerifyEmailAsync(string token);
}