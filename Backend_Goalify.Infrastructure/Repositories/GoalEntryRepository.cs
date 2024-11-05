using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Backend_Goalify.Infrastructure.Repositories
{
    public class GoalEntryRepository : Repository<GoalEntry>, IGoalEntryRepository
    {
        public GoalEntryRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<GoalEntry>> GetUserGoalEntriesAsync(string userId)
        {
            return await _dbSet
                .Where(g => g.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<GoalEntry>> GetPublicGoalEntriesAsync()
        {
            return await _dbSet
                .Where(g => g.IsPublic)
                .ToListAsync();
        }
    }
} 