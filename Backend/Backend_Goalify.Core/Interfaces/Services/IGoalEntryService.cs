using System.Collections.Generic;
using System.Threading.Tasks;
using Backend_Goalify.Core.Entities;

namespace Backend_Goalify.Core.Interfaces
{
    public interface IGoalEntryService
    {
        Task<IEnumerable<GoalEntry>> GetAllGoalEntriesAsync();
        Task<GoalEntry> GetGoalEntryByIdAsync(string id);
        Task<GoalEntry> CreateGoalEntryAsync(GoalEntry goalEntry);
        Task UpdateGoalEntryAsync(GoalEntry goalEntry);
        Task DeleteGoalEntryAsync(string id);
        Task<IEnumerable<GoalEntry>> GetUserGoalEntriesAsync(string userId);
        Task<IEnumerable<GoalEntry>> GetPublicGoalEntriesAsync();

        Task UpdateGoalPriorityAsync(string id, int priority);
        Task UpdateGoalDeadlineAsync(string id, DateTime deadline);
        Task UpdateGoalStatusAsync(string id, string status);
        Task UpdateGoalVisibilityAsync(string id, bool isPublic);
        Task AddGoalTagsAsync(string id, IEnumerable<string> tags);

        /*extras
        */
    }
}