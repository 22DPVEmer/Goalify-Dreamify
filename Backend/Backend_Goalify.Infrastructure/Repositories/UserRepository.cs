using Microsoft.EntityFrameworkCore;
using Backend_Goalify.Infrastructure.Data;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Exceptions;
using Backend_Goalify.Core.Interfaces;

namespace Backend_Goalify.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ApplicationUser>> GetAllAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<ApplicationUser> GetByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id) 
                ?? throw new EntityNotFoundException($"User with ID {id} not found");
        }

        public async Task<ApplicationUser> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        }

        public async Task AddAsync(ApplicationUser entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationUser entity)
        {
            _context.Users.Update(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var user = await GetByIdAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<ApplicationUser> GetUserByChatIdAsync(string chatId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == chatId);
            if (user == null)
            {
                throw new EntityNotFoundException($"User with chat id: {chatId} is not found");
            }
            return user;
        }
    }
}
