using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Goalify.Infrastructure.Repositories
{
    public class GoalEntryRepository(ApplicationDbContext context) :  IGoalEntryRepository
    {
        

        public async Task<IEnumerable<GoalEntry>> GetAllAsync()
        {
            return await context.GoalEntries.ToListAsync();
        }

        public async Task<GoalEntry> GetByIdAsync(string id)
        {
            return await context.GoalEntries.FindAsync(id);
        }

        public async Task AddAsync(GoalEntry entity)
        {
            await context.GoalEntries.AddAsync(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GoalEntry entity)
        {
            context.GoalEntries.Update(entity);
            await context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                context.GoalEntries.Remove(entity);
                await context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<GoalEntry>> GetUserGoalEntriesAsync(string userId)
        {
            return await context.GoalEntries
                .Where(g => g.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GoalEntry>> GetPublicGoalEntriesAsync()
        {
            return await context.GoalEntries
                .Where(g => g.IsPublic)
                .ToListAsync();
        }
    }
} 