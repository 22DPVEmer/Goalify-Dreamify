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

namespace Backend_Goalify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IEmailService _emailService;

        public AuthController(IAuthService authService, IEmailService emailService)
        {
            _authService = authService;
            _emailService = emailService;
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
        public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginModel model)
        {
            var result = await _authService.LoginAsync(model);
            if (!result.success)
                return Unauthorized(new AuthResponse { Success = false, Message = "Invalid credentials" });

            return Ok(new AuthResponse { Success = true, Token = result.token });
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var result = await _authService.RefreshTokenAsync(request.Token);
            
            if (!result.success)
                return Unauthorized(new { message = result.message });

            // Assuming the Result class has a Data property that contains the new token and refresh token
            return Ok(new { Token = result.data.Token, RefreshToken = result.data.RefreshToken });
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
    }
}