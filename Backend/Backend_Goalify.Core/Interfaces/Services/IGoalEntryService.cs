using System.Collections.Generic;
using System.Threading.Tasks;
using Backend_Goalify.Core.Models;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Core.Interfaces
{
    public interface IGoalEntryService
    {
        Task<IEnumerable<GoalEntryModel>> GetAllGoalEntriesAsync(string userid);
        Task<GoalEntryModel> GetGoalEntryByIdAsync(string id);
        Task<GoalEntryModel> CreateGoalEntryAsync(GoalEntryModel goalEntry);
        Task UpdateGoalEntryAsync(GoalEntryModel goalEntry);
        Task DeleteGoalEntryAsync(string id);
        Task<IEnumerable<GoalEntryModel>> GetUserGoalEntriesAsync(string userId);
        Task<IEnumerable<GoalEntryModel>> GetPublicGoalEntriesAsync();
        Task UpdateGoalTagsAsync(string goalId, string userId, IEnumerable<string> tagNames);

        Task UpdateGoalPriorityAsync(string id, GoalPriority priority);
        Task UpdateGoalDeadlineAsync(string id, DateTime deadline);
        Task UpdateGoalStatusAsync(string id, GoalStatus status);
        Task UpdateGoalVisibilityAsync(string id, bool isPublic);
        Task AddGoalTagsAsync(string goalId, string userId, IEnumerable<string> tagNames);
        Task AddGoalToCategoryAsync(string goalId, string categoryId);
        Task UpdateGoalCategoriesAsync(string goalId, string categoryId);

    }
}