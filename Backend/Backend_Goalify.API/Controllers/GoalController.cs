using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Backend_Goalify.Core.Entities.Enums;
using Backend_Goalify.Core.Models.Enums;
using Backend_Goalify.Core.Models;

namespace Backend_Goalify.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class GoalController : ControllerBase
    {
        private readonly IGoalEntryService _goalService;

        public GoalController(IGoalEntryService goalService)
        {
            _goalService = goalService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GoalEntryModel>>> GetUserGoals()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var goals = await _goalService.GetUserGoalEntriesAsync(userId);
            return Ok(goals);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GoalEntry>> GetGoal(string id)
        {
            var goal = await _goalService.GetGoalEntryByIdAsync(id);
            if (goal == null)
                return NotFound();

            return Ok(goal);
        }

        [HttpPost]
        public async Task<ActionResult<GoalEntry>> CreateGoal([FromBody] Dictionary<string, object> goalData)
        {
            try 
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var goal = new GoalEntryModel();
                
                // Set system-managed properties with UTC dates
                goal.Id = Guid.NewGuid().ToString();
                goal.UserId = userId;
                goal.CreatedAt = DateTime.UtcNow;
                goal.UpdatedAt = DateTime.UtcNow;
                goal.IsActive = true;
                goal.Status = Core.Models.Enums.GoalStatus.InProgress; // Set default status

                // Set user-provided properties with validation
                if (!goalData.TryGetValue("title", out var titleObj) || string.IsNullOrEmpty(titleObj?.ToString()))
                {
                    return BadRequest(new { message = "Title is required" });
                }
                goal.Title = titleObj.ToString();

                if (goalData.TryGetValue("description", out var descObj))
                {
                    goal.Description = descObj?.ToString();
                }

                if (goalData.TryGetValue("isPublic", out var isPublicObj) && isPublicObj != null)
                {
                    if (bool.TryParse(isPublicObj.ToString(), out bool isPublic))
                    {
                        goal.IsPublic = isPublic;
                    }
                }

                // Handle deadline with explicit UTC conversion
                if (goalData.TryGetValue("deadline", out var deadlineObj) && deadlineObj != null)
                {
                    if (DateTime.TryParse(deadlineObj.ToString(), out DateTime deadline))
                    {
                        // Convert to UTC if not already
                        goal.DueDate = deadline.Kind != DateTimeKind.Utc 
                            ? DateTime.SpecifyKind(deadline, DateTimeKind.Utc)
                            : deadline;
                    }
                }

                // Handle priority as string and parse to enum
                if (goalData.TryGetValue("priority", out var priorityObj) && priorityObj != null)
                {
                    var priorityStr = priorityObj.ToString().Trim();
                    if (Enum.TryParse<Backend_Goalify.Core.Models.Enums.GoalPriority>(priorityStr, true, out var priority))
                    {
                        goal.Priority = priority;
                    }
                    else
                    {
                        var validPriorities = string.Join(", ", Enum.GetNames(typeof(Backend_Goalify.Core.Models.Enums.GoalPriority)));
                        return BadRequest(new { message = $"Invalid priority value. Valid values are: {validPriorities}" });
                    }
                }

                // Handle status if provided
                if (goalData.TryGetValue("status", out var statusObj) && statusObj != null)
                {
                    if (Enum.TryParse<Core.Models.Enums.GoalStatus>(statusObj.ToString(), true, out var status))
                    {
                        goal.Status = status;
                    }
                }

                var createdGoal = await _goalService.CreateGoalEntryAsync(goal);
                return CreatedAtAction(nameof(GetGoal), new { id = createdGoal.Id }, createdGoal);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error creating goal", error = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(string id, [FromBody] GoalEntryModel goal)
        {
            if (id != goal.Id)
                return BadRequest();

            await _goalService.UpdateGoalEntryAsync(goal);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(string id)
        {
            await _goalService.DeleteGoalEntryAsync(id);
            return NoContent();
        }

        [HttpPatch("{id}/priority")]
        public async Task<IActionResult> UpdatePriority(string id, [FromBody] Backend_Goalify.Core.Models.Enums.GoalPriority priority)
        {
            await _goalService.UpdateGoalPriorityAsync(id, priority);
            return NoContent();
        }

        [HttpPatch("{id}/deadline")]
        public async Task<IActionResult> UpdateDeadline(string id, [FromBody] DateTime deadline)
        {
            await _goalService.UpdateGoalDeadlineAsync(id, deadline);
            return NoContent();
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] Backend_Goalify.Core.Models.Enums.GoalStatus status)
        {
            await _goalService.UpdateGoalStatusAsync(id, status);
            return NoContent();
        }

        [HttpPatch("{id}/visibility")]
        public async Task<IActionResult> UpdateVisibility(string id, [FromBody] bool isPublic)
        {
            await _goalService.UpdateGoalVisibilityAsync(id, isPublic);
            return NoContent();
        }

        [HttpPost("{goalId}/tags")]
        public async Task<IActionResult> AddTags(string goalId, [FromBody] IEnumerable<string> tags)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                if (!tags?.Any() ?? true)
                    return BadRequest("No tags provided");

                await _goalService.AddGoalTagsAsync(goalId, userId, tags);
                return Ok(new { message = "Tags added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }


        
    }
}