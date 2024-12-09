using Backend_Goalify.Core.Entities;
using System.Threading.Tasks;
using Backend_Goalify.Core.Models;

public interface IAuthService
{
    Task<(bool success, string token)> LoginAsync(LoginModel model);
    Task<(bool success, string message)> RegisterAsync(RegisterModel model);

    Task<Result> AssignRoleAsync(string userId, string role);
    Task<Result> RefreshTokenAsync(string token);
    //string GenerateJwtToken(ApplicationUser user);
} 