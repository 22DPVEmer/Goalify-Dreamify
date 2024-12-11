using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Backend_Goalify.Application.Services
{
    public class GoalEntryService : IGoalEntryService
    {
        private readonly IGoalEntryRepository _goalEntryRepository;

        public GoalEntryService(IGoalEntryRepository goalEntryRepository)
        {
            _goalEntryRepository = goalEntryRepository;
        }

        public async Task<IEnumerable<GoalEntry>> GetAllGoalEntriesAsync()
        {
            return await _goalEntryRepository.GetAllAsync();
        }

        public async Task<GoalEntry> GetGoalEntryByIdAsync(string id)
        {
            return await _goalEntryRepository.GetByIdAsync(id);
        }

        public async Task<GoalEntry> CreateGoalEntryAsync(GoalEntry goalEntry)
        {
            return await _goalEntryRepository.AddAsync(goalEntry);
        }

        public async Task UpdateGoalEntryAsync(GoalEntry goalEntry)
        {
            await _goalEntryRepository.UpdateAsync(goalEntry);
        }

        public async Task DeleteGoalEntryAsync(string id)
        {
            await _goalEntryRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<GoalEntry>> GetUserGoalEntriesAsync(string userId)
        {
            return await _goalEntryRepository.GetUserGoalEntriesAsync(userId);
        }

        public async Task<IEnumerable<GoalEntry>> GetPublicGoalEntriesAsync()
        {
            return await _goalEntryRepository.GetPublicGoalEntriesAsync();
        }

        public async Task UpdateGoalPriorityAsync(string id, int priority)
        {
            var goal = await _goalEntryRepository.GetByIdAsync(id);
            goal.Priority = priority;
            await _goalEntryRepository.UpdateAsync(goal);
        }

        public async Task UpdateGoalDeadlineAsync(string id, DateTime deadline)
        {
            var goal = await _goalEntryRepository.GetByIdAsync(id);
            goal.Deadline = deadline;
            await _goalEntryRepository.UpdateAsync(goal);
        }

        public async Task UpdateGoalStatusAsync(string id, string status)
        {
            var goal = await _goalEntryRepository.GetByIdAsync(id);
            goal.Status = status;
            await _goalEntryRepository.UpdateAsync(goal);
        }

        public async Task UpdateGoalVisibilityAsync(string id, bool isPublic)
        {
            var goal = await _goalEntryRepository.GetByIdAsync(id);
            goal.IsPublic = isPublic;
            await _goalEntryRepository.UpdateAsync(goal);
        }

        public async Task AddGoalTagsAsync(string id, IEnumerable<string> tags)
        {
            var goal = await _goalEntryRepository.GetByIdAsync(id);
            goal.Tags.AddRange(tags);
            await _goalEntryRepository.UpdateAsync(goal);
        }
    }
}