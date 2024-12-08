using Backend_Goalify.Core.Entities;
using System.Threading.Tasks;
using Backend_Goalify.Core.Models;

public interface IAuthService
{
    Task<(bool success, string token)> LoginAsync(LoginModel model);
    Task<(bool success, string message)> RegisterAsync(RegisterModel model);
    //string GenerateJwtToken(ApplicationUser user);
} 