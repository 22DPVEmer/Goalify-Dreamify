using Microsoft.EntityFrameworkCore;
using Backend_Goalify.Infrastructure.Data;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Exceptions;
using Backend_Goalify.Core.Interfaces;
namespace SisyphusChat.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    public async Task AddAsync(ApplicationUser entity)
    {
        await context.Users.AddAsync(entity);
        await context.SaveChangesAsync();
    }

    public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
    {
        return await context.Users.ToListAsync();
    }

    public async Task<ApplicationUser> GetByIdAsync(string id)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(g => g.Id == id);

        if (user == null)
        {
            throw new EntityNotFoundException("Entity not found");
        }

        return user;
    }

    public async Task<ApplicationUser> GetByUsernameAsync(string userName)
    {
        var user = await context.Users
            .FirstOrDefaultAsync(g => g.UserName == userName);


        return user;
    }

    public async Task DeleteAsync(string id)
    {
        var user = await context.Users.FindAsync(id);

        if (user == null)
        {
            throw new EntityNotFoundException("Entity not found");
        }

        context.Users.Remove(user);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(ApplicationUser entity)
    {
        
        if (entity == null)
        {
            throw new EntityNotFoundException("Entity not found");
        }

        context.Users.Update(entity);
        await context.SaveChangesAsync();
    }

    public async Task<ApplicationUser> GetUserByChatIdAsync(string chatId)
    {
        var user = await context.Users
            .FirstOrDefaultAsync();

        if (user == null)
        {
            throw new EntityNotFoundException($"User with chat id: {chatId} is not found");
        }

        return user;
    }
}
