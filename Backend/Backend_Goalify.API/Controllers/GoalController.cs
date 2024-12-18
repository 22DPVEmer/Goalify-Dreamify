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
        public async Task<ActionResult<GoalEntry>> CreateGoal([FromBody] GoalEntryModel goal)
        {
            goal.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdGoal = await _goalService.CreateGoalEntryAsync(goal);
            return CreatedAtAction(nameof(GetGoal), new { id = createdGoal.Id }, createdGoal);
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

        [HttpPost("{id}/tags")]
        public async Task<IActionResult> AddTags(string id, [FromBody] IEnumerable<string> tags)
        {
            await _goalService.AddGoalTagsAsync(id, tags);
            return NoContent();
        }
    }
}