using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Core.Models;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace Backend_Goalify.API.Controllers
{
    [Authorize] // Keep this!
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet("profile")]
        public async Task<ActionResult<UserModel>> GetProfile()
        {
            try
            {
                // Debug Authentication Status
                _logger.LogInformation($"Is User Authenticated: {User.Identity?.IsAuthenticated}");
                _logger.LogInformation($"Authentication Type: {User.Identity?.AuthenticationType}");
                
                // Debug Cookie
                var authCookie = Request.Cookies["auth_token"];
                _logger.LogInformation($"Auth Cookie Present: {!string.IsNullOrEmpty(authCookie)}");
                
                // Debug Headers
                var authHeader = Request.Headers["Authorization"].FirstOrDefault();
                _logger.LogInformation($"Authorization Header: {authHeader}");

                // Debug Claims
                foreach (var claim in User.Claims)
                {
                    _logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
                }

                // Continue with existing logic
                var token = Request.Cookies["auth_token"];
                _logger.LogInformation($"Received token in profile request: {token?.Substring(0, Math.Min(token?.Length ?? 0, 20))}...");
                
                var identity = User.Identity;
                _logger.LogInformation($"IsAuthenticated: {identity?.IsAuthenticated}");

                foreach (var claim in User.Claims)
                {
                    _logger.LogInformation($"Claim: {claim.Type} = {claim.Value}");
                }

                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                _logger.LogInformation($"Found email in claims: {userEmail}");

                if (string.IsNullOrEmpty(userEmail))
                {
                    _logger.LogWarning("Unauthorized access attempt - no email found in claims");
                    return Unauthorized(new { message = "User not authenticated" });
                }

                var user = await _userService.GetProfileAsync(userEmail);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {userEmail}");
                    return NotFound(new { message = "User not found" });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetProfile with detailed diagnostics");
                return StatusCode(500, new { 
                    message = "An error occurred while fetching the profile",
                    error = ex.Message,
                    stack = ex.StackTrace 
                });
            }
        }
    }
}
