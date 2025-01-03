using Microsoft.EntityFrameworkCore;
using Backend_Goalify.Infrastructure.Data;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Exceptions;
using Backend_Goalify.Core.Interfaces;

namespace Backend_Goalify.Infrastructure.Repositories
{
    public class TagRepository(ApplicationDbContext context) : ITagRepository
    {
        public async Task AddAsync(Tag entity)
        {
            await context.Tags.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Tag>> GetAllAsync()
        {
            return await context.Tags.ToListAsync();
        }

        public async Task<Tag> GetByIdAsync(string id)
        {
            var tag = await context.Tags
                .FirstOrDefaultAsync(g => g.Id == id);

            if (tag == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }

            return tag;
        }

        public async Task DeleteAsync(string id)
        {
            var tag = await context.Tags.FindAsync(id);

            if (tag == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }

            context.Tags.Remove(tag);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Tag entity)
        {
            if (entity == null)
            {
                throw new EntityNotFoundException("Entity not found");
            }

            context.Tags.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task<Tag?> FindByNameAsync(string name)
        {
            return await context.Tags
                .FirstOrDefaultAsync(t => t.Name.ToLower() == name.ToLower());
        }
    }
}