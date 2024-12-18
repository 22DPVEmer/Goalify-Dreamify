using Backend_Goalify.Core.Entities;


namespace Backend_Goalify.Core.Interfaces
{

    public interface IUserRepository : IRepository<ApplicationUser>
    {
    new Task AddAsync(ApplicationUser entity);
    Task<ApplicationUser> GetByUsernameAsync(string username);

    Task<ApplicationUser> GetUserByChatIdAsync(string chatId);

    }

}
 