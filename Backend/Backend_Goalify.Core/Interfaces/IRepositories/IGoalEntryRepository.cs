using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;

namespace Backend_Goalify.Core.Interfaces{
    public interface IGoalEntryRepository : IRepository<GoalEntry>
    {
        Task<IEnumerable<GoalEntry>> GetUserGoalEntriesAsync(string userId);
        Task<IEnumerable<GoalEntry>> GetPublicGoalEntriesAsync();
        Task<GoalEntry> GetByIdWithCategoriesAsync(string id);
    } 
}
