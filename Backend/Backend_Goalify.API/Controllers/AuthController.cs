using Backend_Goalify.Core.Models;
using Backend_Goalify.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Backend_Goalify.Application.Services;
using Microsoft.AspNetCore.Http;

namespace Backend_Goalify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        //private readonly IEmailService _emailService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
            //_emailService = emailService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterModel model)
        {
            var result = await _authService.RegisterAsync(model);
            if (!result.success)
                return BadRequest(new AuthResponse { Success = false, Message = result.message });

            return Ok(new AuthResponse { Success = true, Message = result.message });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var (success, token, user) = await _authService.LoginAsync(model);
            
            if (!success)
                return Unauthorized(new { success = false, message = "Invalid email or password" });

            // Set auth cookie with proper options
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            Response.Cookies.Append("auth_token", token, cookieOptions);

            // Map user to UserModel
            var userModel = new UserModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                IsActive = user.IsActive,
                IsAdmin = user.IsAdmin
            };

            return Ok(new { 
                success = true,
                user = userModel
            });
        }
        /*

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.Token);
            
            if (!result.success)
                return Unauthorized(new { message = result.message });

            
            return Ok(new { Token = result.data.Token, RefreshToken = result.data.RefreshToken });
        }
        */
        [HttpGet("check")]
        public IActionResult CheckAuth()
        {
            var token = Request.Cookies["auth_token"];
            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized(new { message = "Not authenticated" });
            }
            return Ok(new { message = "Authenticated" });
        }

        /*
        
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
        {
            var result = await _authService.GeneratePasswordResetTokenAsync(request.Email);
            if (!result.success)
                return BadRequest(new { message = result.message });

            await _emailService.SendPasswordResetEmailAsync(request.Email, result.token);
            return Ok(new { message = "Password reset email sent" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = "Password reset successful" });
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyEmailRequest request)
        {
            var result = await _authService.VerifyEmailAsync(request.Token);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = "Email verified successfully" });
        }
    */
        [Authorize(Roles = "Admin")]
        [HttpPost("assign-role")]
        public async Task<IActionResult> AssignRole([FromBody] AssignRoleRequest request)
        {
            var result = await _authService.AssignRoleAsync(request.UserId, request.Role);
            if (!result.success)
                return BadRequest(new { message = result.message });

            return Ok(new { message = "Role assigned successfully" });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("auth_token", new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            });
            return Ok(new { message = "Logged out successfully" });
        }
    }
}