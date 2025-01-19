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
        private readonly IFirebaseStorageService _firebaseStorageService;

        public UserController(IUserService userService, ILogger<UserController> logger, IFirebaseStorageService firebaseStorageService)
        {
            _userService = userService;
            _logger = logger;
            _firebaseStorageService = firebaseStorageService;
        }
        [HttpDelete("delete")]
        public async Task<ActionResult<UserModel>> DeleteUser(string id)
        {
            await _userService.DeleteUserAsync(id);
            return Ok();
        }
        [HttpPut("update")]
        public async Task<ActionResult<UserModel>> UpdateUser(UserModel userModel)
        {
            var updatedUser = await _userService.UpdateUserAsync(userModel);
            return Ok(updatedUser);
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
        [HttpPost("profile-picture")]
        public async Task<IActionResult> UploadProfilePicture(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return BadRequest("No file uploaded");

                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized();

                var user = await _userService.GetProfileAsync(userEmail);
                if (user == null)
                    return NotFound("User not found");

                // Delete old profile picture if exists
                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    try
                    {
                        await _firebaseStorageService.DeleteProfilePictureAsync(user.ProfilePicture);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning($"Failed to delete old profile picture: {ex.Message}");
                        // Continue with upload even if delete fails
                    }
                }

                using var stream = file.OpenReadStream();
                var imageUrl = await _firebaseStorageService.UploadProfilePictureAsync(
                    user.Id, 
                    stream, 
                    file.ContentType
                );

                // Update user profile URL in database
                user.ProfilePicture = imageUrl;
                await _userService.UpdateUserAsync(user);

                return Ok(new { 
                    imageUrl,
                    message = "Profile picture updated successfully"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading profile picture");
                return StatusCode(500, new { error = ex.Message });
            }
        }
        [HttpDelete("profile-picture")]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            try
            {
                var userEmail = User.FindFirst(ClaimTypes.Email)?.Value;
                if (string.IsNullOrEmpty(userEmail))
                    return Unauthorized();

                var user = await _userService.GetProfileAsync(userEmail);
                if (user == null)
                    return NotFound("User not found");

                if (!string.IsNullOrEmpty(user.ProfilePicture))
                {
                    await _firebaseStorageService.DeleteProfilePictureAsync(user.ProfilePicture);
                    user.ProfilePicture = null;
                    await _userService.UpdateUserAsync(user);
                }

                return Ok(new { message = "Profile picture removed successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting profile picture");
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
