using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
/*
namespace Backend_Goalify.Infrastructure.Repositories
{
    public class RoleRepository :  IRoleRepository
    {
        public RoleRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<Role> GetRoleByIdAsync(string id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            await _dbSet.AddAsync(role);
            await _context.SaveChangesAsync();
            return role;
        }

        public async Task UpdateRoleAsync(Role role)
        {
            _dbSet.Update(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteRoleAsync(string id)
        {
            var role = await _dbSet.FindAsync(id);
            _dbSet.Remove(role);
            await _context.SaveChangesAsync();
        }
    }
}
*/