using Backend_Goalify.Core.Models;
using Backend_Goalify.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
namespace Backend_Goalify.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
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
    }
} 