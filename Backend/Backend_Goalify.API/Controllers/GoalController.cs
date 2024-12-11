using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<IEnumerable<GoalEntry>>> GetUserGoals()
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
        public async Task<ActionResult<GoalEntry>> CreateGoal([FromBody] GoalEntry goal)
        {
            goal.UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var createdGoal = await _goalService.CreateGoalEntryAsync(goal);
            return CreatedAtAction(nameof(GetGoal), new { id = createdGoal.Id }, createdGoal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGoal(string id, [FromBody] GoalEntry goal)
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
    }
}