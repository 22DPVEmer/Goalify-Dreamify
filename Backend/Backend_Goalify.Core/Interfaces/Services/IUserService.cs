using Backend_Goalify.Core.Models;
namespace Backend_Goalify.Core.Interfaces
{
public interface IUserService
{
    Task<UserModel> GetUserByIdAsync(string id);
    Task<IEnumerable<UserModel>> GetAllUsersAsync();
    Task<UserModel> UpdateUserAsync(UserModel userModel);
    Task DeleteUserAsync(string id);
    }  
} 