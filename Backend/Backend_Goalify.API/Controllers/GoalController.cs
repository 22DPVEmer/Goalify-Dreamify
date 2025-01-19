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
                
                // Add debug logging
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                var cookie = Request.Cookies["auth_token"];
                Console.WriteLine($"Auth Header: {authHeader}");
                Console.WriteLine($"Auth Cookie: {cookie}");
                Console.WriteLine($"User ID from claims: {userId}");
                Console.WriteLine($"Is authenticated: {User.Identity?.IsAuthenticated}");
                
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
                if (goalData.TryGetValue("dueDate", out var deadlineObj) && deadlineObj != null)
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

                // Handle status
                if (goalData.TryGetValue("status", out var statusObj) && statusObj != null)
                {
                    // Try to parse as integer first (for numeric enum values)
                    if (int.TryParse(statusObj.ToString(), out int statusInt))
                    {
                        goal.Status = (Core.Models.Enums.GoalStatus)statusInt;
                    }
                    // If not a number, try parsing as string enum
                    else if (Enum.TryParse<Core.Models.Enums.GoalStatus>(statusObj.ToString(), true, out var status))
                    {
                        goal.Status = status;
                    }
                    else
                    {
                        var validStatuses = string.Join(", ", Enum.GetNames(typeof(Core.Models.Enums.GoalStatus)));
                        return BadRequest(new { message = $"Invalid status value. Valid values are: {validStatuses}" });
                    }
                }

                // Handle isActive
                if (goalData.TryGetValue("isActive", out var isActiveObj) && isActiveObj != null)
                {
                    if (bool.TryParse(isActiveObj.ToString(), out bool isActive))
                    {
                        goal.IsActive = isActive;
                    }
                }

                // Handle isCompleted
                if (goalData.TryGetValue("isCompleted", out var isCompletedObj) && isCompletedObj != null)
                {
                    if (bool.TryParse(isCompletedObj.ToString(), out bool isCompleted))
                    {
                        goal.IsCompleted = isCompleted;
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
        public async Task<IActionResult> UpdateGoal(string id, [FromBody] Dictionary<string, object> goalData)
        {
            try
            {
                Console.WriteLine($"Updating goal with ID: {id}");
                Console.WriteLine($"Received goal data: {System.Text.Json.JsonSerializer.Serialize(goalData)}");

                if (!goalData.TryGetValue("id", out var goalId) || goalId?.ToString() != id)
                {
                    return BadRequest(new { message = "ID mismatch or missing" });
                }

                var goal = await _goalService.GetGoalEntryByIdAsync(id);
                if (goal == null)
                {
                    return NotFound(new { message = "Goal not found" });
                }

                // Update basic properties
                if (goalData.TryGetValue("title", out var titleObj) && !string.IsNullOrEmpty(titleObj?.ToString()))
                {
                    goal.Title = titleObj.ToString();
                }

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
                if (goalData.TryGetValue("dueDate", out var deadlineObj) && deadlineObj != null)
                {
                    if (DateTime.TryParse(deadlineObj.ToString(), out DateTime deadline))
                    {
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

                // Handle status
                if (goalData.TryGetValue("status", out var statusObj) && statusObj != null)
                {
                    // Try to parse as integer first (for numeric enum values)
                    if (int.TryParse(statusObj.ToString(), out int statusInt))
                    {
                        goal.Status = (Core.Models.Enums.GoalStatus)statusInt;
                    }
                    // If not a number, try parsing as string enum
                    else if (Enum.TryParse<Core.Models.Enums.GoalStatus>(statusObj.ToString(), true, out var status))
                    {
                        goal.Status = status;
                    }
                    else
                    {
                        var validStatuses = string.Join(", ", Enum.GetNames(typeof(Core.Models.Enums.GoalStatus)));
                        return BadRequest(new { message = $"Invalid status value. Valid values are: {validStatuses}" });
                    }
                }

                // Handle isActive
                if (goalData.TryGetValue("isActive", out var isActiveObj) && isActiveObj != null)
                {
                    if (bool.TryParse(isActiveObj.ToString(), out bool isActive))
                    {
                        goal.IsActive = isActive;
                    }
                }

                // Handle isCompleted
                if (goalData.TryGetValue("isCompleted", out var isCompletedObj) && isCompletedObj != null)
                {
                    if (bool.TryParse(isCompletedObj.ToString(), out bool isCompleted))
                    {
                        goal.IsCompleted = isCompleted;
                    }
                }

                await _goalService.UpdateGoalEntryAsync(goal);
                return NoContent();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating goal: {ex}");
                return StatusCode(500, new { message = "Error updating goal", error = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGoal(string id)
        {
            try
            {
                var goal = await _goalService.GetGoalEntryByIdAsync(id);
                if (goal == null)
                {
                    return NotFound(new { message = "Goal not found" });
                }

                await _goalService.DeleteGoalEntryAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error deleting goal", error = ex.Message });
            }
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

        public class StatusUpdateRequest
        {
            public int Value { get; set; }
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(string id, [FromBody] StatusUpdateRequest request)
        {
            try
            {
                if (!Enum.IsDefined(typeof(Core.Models.Enums.GoalStatus), request.Value))
                {
                    var validStatuses = string.Join(", ", Enum.GetNames(typeof(Core.Models.Enums.GoalStatus)));
                    return BadRequest(new { message = $"Invalid status value. Valid values are: {validStatuses}" });
                }

                var goal = await _goalService.GetGoalEntryByIdAsync(id);
                if (goal == null)
                {
                    return NotFound(new { message = "Goal not found" });
                }

                var newStatus = (Core.Models.Enums.GoalStatus)request.Value;
                goal.Status = newStatus;
                await _goalService.UpdateGoalEntryAsync(goal);
                
                return Ok(new { 
                    message = $"Status updated successfully to {newStatus}",
                    newStatus = newStatus.ToString()
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating goal status: {ex}");
                return StatusCode(500, new { message = "Error updating goal status", error = ex.Message });
            }
        }

        [HttpPatch("{id}/visibility")]
        public async Task<IActionResult> UpdateVisibility(string id, [FromBody] bool isPublic)
        {
            await _goalService.UpdateGoalVisibilityAsync(id, isPublic);
            return NoContent();
        }

        [HttpPost("{goalId}/tags")]
        public async Task<IActionResult> AddTags(string goalId, [FromBody] string[] tags)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                if (tags == null || !tags.Any())
                    return BadRequest("No tags provided");

                await _goalService.AddGoalTagsAsync(goalId, userId, tags);
                return Ok(new { message = "Tags added successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPost("{goalId}/categories/{categoryId}")]
        public async Task<IActionResult> AddGoalToCategory(string goalId, string categoryId)
        {
            try
            {
                await _goalService.AddGoalToCategoryAsync(goalId, categoryId);
                return Ok(new { message = "Goal added to category successfully" });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error adding goal to category", error = ex.Message });
            }
        }

        [HttpPut("{goalId}/tags")]
        public async Task<IActionResult> UpdateTags(string goalId, [FromBody] string[] tags)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized();

                await _goalService.UpdateGoalTagsAsync(goalId, userId, tags);
                return Ok(new { message = "Tags updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = ex.Message });
            }
        }

        [HttpPut("{goalId}/categories")]
        public async Task<IActionResult> UpdateCategories(string goalId, [FromBody] string categoryId)
        {
            try
            {
                await _goalService.UpdateGoalCategoriesAsync(goalId, categoryId);
                return Ok(new { message = "Categories updated successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error updating goal categories", error = ex.Message });
            }
        }

        public class TagsRequest
        {
            public IEnumerable<string> tags { get; set; }
        }

        


        
    }
}