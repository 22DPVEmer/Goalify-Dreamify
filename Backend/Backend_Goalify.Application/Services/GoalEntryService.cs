using AutoMapper;
using Backend_Goalify.Core.Models;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Backend_Goalify.Core.Models.Enums;

namespace Backend_Goalify.Application.Services
{
    public class GoalEntryService : IGoalEntryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GoalEntryService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<GoalEntryModel>> GetAllGoalEntriesAsync(string userid)
        {
            var entries = await _unitOfWork.GoalEntryRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<GoalEntryModel>>(entries);
        }

        public async Task<GoalEntryModel> GetGoalEntryByIdAsync(string id)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(id);
            return _mapper.Map<GoalEntryModel>(entry);
        }

        public async Task<GoalEntryModel> CreateGoalEntryAsync(GoalEntryModel goalEntryModel)
        {
            var entry = _mapper.Map<GoalEntry>(goalEntryModel);
            var result = await _unitOfWork.GoalEntryRepository.AddAsync(entry);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GoalEntryModel>(result);
        }

        public async Task UpdateGoalEntryAsync(GoalEntryModel goalEntryModel)
        {
            var entry = _mapper.Map<GoalEntry>(goalEntryModel);
            await _unitOfWork.GoalEntryRepository.UpdateAsync(entry);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteGoalEntryAsync(string id)
        {
            await _unitOfWork.GoalEntryRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<GoalEntryModel>> GetUserGoalEntriesAsync(string userId)
        {
            var entries = await _unitOfWork.GoalEntryRepository.GetUserGoalEntriesAsync(userId);
            return _mapper.Map<IEnumerable<GoalEntryModel>>(entries);
        }

        public async Task<IEnumerable<GoalEntryModel>> GetPublicGoalEntriesAsync()
        {
            var entries = await _unitOfWork.GoalEntryRepository.GetPublicGoalEntriesAsync();
            return _mapper.Map<IEnumerable<GoalEntryModel>>(entries);
        }

        public async Task UpdateGoalPriorityAsync(string id, GoalPriority priority)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(id);
            entry.Priority = (Backend_Goalify.Core.Entities.Enums.GoalPriority)priority;
            await _unitOfWork.GoalEntryRepository.UpdateAsync(entry);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateGoalDeadlineAsync(string id, DateTime deadline)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(id);
            entry.Deadline = deadline;
            await _unitOfWork.GoalEntryRepository.UpdateAsync(entry);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateGoalStatusAsync(string id, GoalStatus status)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(id);
            entry.Status = (Backend_Goalify.Core.Entities.Enums.GoalStatus)status;
            await _unitOfWork.GoalEntryRepository.UpdateAsync(entry);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateGoalVisibilityAsync(string id, bool isPublic)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(id);
            entry.IsPublic = isPublic;
            await _unitOfWork.GoalEntryRepository.UpdateAsync(entry);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddGoalTagsAsync(string id, IEnumerable<string> tags)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(id);
            if (entry == null)
            {
                throw new KeyNotFoundException("Goal entry not found.");
            }

            foreach (var tagName in tags)
            {
                var tag = new Tag { Name = tagName };
                entry.Tags.Add(tag);
            }

            await _unitOfWork.GoalEntryRepository.UpdateAsync(entry);
            await _unitOfWork.SaveAsync();
        }
    }
}