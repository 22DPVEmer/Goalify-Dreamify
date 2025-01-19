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
            await _unitOfWork.GoalEntryRepository.AddAsync(entry);
            await _unitOfWork.SaveAsync();
            return _mapper.Map<GoalEntryModel>(entry);
        }

        public async Task UpdateGoalEntryAsync(GoalEntryModel goalEntryModel)
        {
            // Get the existing entity from the database
            var existingEntry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(goalEntryModel.Id);
            if (existingEntry == null)
            {
                throw new KeyNotFoundException($"Goal with ID {goalEntryModel.Id} not found");
            }

            // Update the existing entity's properties
            existingEntry.Title = goalEntryModel.Title;
            existingEntry.Description = goalEntryModel.Description;
            existingEntry.Deadline = goalEntryModel.DueDate;
            existingEntry.Priority = (Core.Entities.Enums.GoalPriority)goalEntryModel.Priority;
            existingEntry.Status = (Core.Entities.Enums.GoalStatus)goalEntryModel.Status;
            existingEntry.IsPublic = (bool)goalEntryModel.IsPublic;
            existingEntry.IsActive = (bool)goalEntryModel.IsActive;
            existingEntry.IsCompleted = (bool)goalEntryModel.IsCompleted;
            existingEntry.UpdatedAt = DateTime.UtcNow;

            // Update the existing entity
            await _unitOfWork.GoalEntryRepository.UpdateAsync(existingEntry);
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

        public async Task AddGoalTagsAsync(string goalId, string userId, IEnumerable<string> tagNames)
        {
            var entry = await _unitOfWork.GoalEntryRepository.GetByIdAsync(goalId);
            if (entry == null)
            {
                throw new KeyNotFoundException("Goal entry not found.");
            }

            foreach (var tagName in tagNames)
            {
                // Try to find existing tag first
                var existingTag = await _unitOfWork.TagRepository.FindByNameAsync(tagName);
                
                if (existingTag == null)
                {
                    // Create new tag if it doesn't exist
                    existingTag = new Tag 
                    { 
                        Name = tagName,
                        UserId = userId,
                        UsageCount = 1
                    };
                    await _unitOfWork.TagRepository.AddAsync(existingTag);
                }
                else
                {
                    existingTag.UsageCount++;
                }

                // Add the tag to the goal's tags collection
                entry.Tags.Add(existingTag);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task AddGoalToCategoryAsync(string goalId, string categoryId)
        {
            var goal = await _unitOfWork.GoalEntryRepository.GetByIdAsync(goalId);
            if (goal == null)
            {
                throw new KeyNotFoundException("Goal not found");
            }

            var category = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
            if (category == null)
            {
                throw new KeyNotFoundException("Category not found");
            }

            goal.Categories.Add(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateGoalTagsAsync(string goalId, string userId, IEnumerable<string> tags)
        {
            var goal = await _unitOfWork.GoalEntryRepository.GetByIdAsync(goalId);
            if (goal == null || goal.UserId != userId)
                throw new UnauthorizedAccessException("User not authorized to update this goal's tags");

            // Create a list to hold the Tag entities
            var tagEntities = new List<Tag>();

            foreach (var tagName in tags)
            {
                // Try to find existing tag first
                var existingTag = await _unitOfWork.TagRepository.FindByNameAsync(tagName);
                
                if (existingTag == null)
                {
                    // Create new tag if it doesn't exist
                    existingTag = new Tag 
                    { 
                        Name = tagName,
                        UserId = userId,
                        UsageCount = 1
                    };
                    await _unitOfWork.TagRepository.AddAsync(existingTag);
                }
                else
                {
                    existingTag.UsageCount++;
                }

                // Add the tag to the list of tag entities
                tagEntities.Add(existingTag);
            }

            // Assign the list of Tag entities to the goal's Tags property
            goal.Tags = tagEntities;
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateGoalCategoriesAsync(string goalId, string categoryId)
        {
            var goal = await _unitOfWork.GoalEntryRepository.GetByIdWithCategoriesAsync(goalId);
            if (goal == null)
                throw new KeyNotFoundException("Goal not found");

            // Clear all existing categories first to ensure we don't have duplicates
            goal.Categories.Clear();
            await _unitOfWork.SaveAsync();

            // If we have a new category ID, add it
            if (!string.IsNullOrEmpty(categoryId))
            {
                var newCategory = await _unitOfWork.CategoryRepository.GetByIdAsync(categoryId);
                if (newCategory != null)
                {
                    goal.Categories.Add(newCategory);
                    await _unitOfWork.SaveAsync();
                }
            }
        }
    }
}