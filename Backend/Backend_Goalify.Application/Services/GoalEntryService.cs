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
    }
} 