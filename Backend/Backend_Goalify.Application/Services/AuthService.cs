using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Models;
using Backend_Goalify.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; // Add this line if it's missing

namespace Backend_Goalify.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtService _jwtService;

        public AuthService(
            UserManager<ApplicationUser> userManager,
            IJwtService jwtService)
        {
            _userManager = userManager;
            _jwtService = jwtService;
        }

        public async Task<Result> RefreshTokenAsync(string token)
        {      
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == token);
            
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return new Result { success = false, message = "Invalid or expired refresh token" };

            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(15);
            await _userManager.UpdateAsync(user);

            // Create an instance of TokenData and assign it to the data property
            var tokenData = new TokenData
            {
                Token = newToken,
                RefreshToken = newRefreshToken
            };

            return new Result { success = true, data = tokenData }; // Return the Result with TokenData
        }

        public async Task<(bool success, string token, ApplicationUser user)> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return (false, null!, null!);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            user = await _userManager.FindByIdAsync(user.Id);

            var token = _jwtService.GenerateToken(user);
            return (true, token, user);
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterModel model)  // Return type matches interface
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
                return (false, "User with this email already exists.");

            var user = new ApplicationUser
            {
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            
            if (!result.Succeeded)
                return (false, string.Join(", ", result.Errors.Select(e => e.Description)));

            // Assign default Basic role
            var roleResult = await _userManager.AddToRoleAsync(user, "Basic");
            
            if (!roleResult.Succeeded)
                return (false, "User created but role assignment failed");

            return (true, "User created successfully with Basic role");
        }


        public async Task<(bool success, string token, string message)> GeneratePasswordResetTokenAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return (false, null, "User not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            user.PasswordResetToken = token;
            user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);
            await _userManager.UpdateAsync(user);

            return (true, token, null);
        }

        public async Task<(bool success, string message)> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.PasswordResetToken == request.Token);

            if (user == null || user.PasswordResetTokenExpiry <= DateTime.UtcNow)
                return (false, "Invalid or expired reset token");

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);
            if (!result.Succeeded)
                return (false, "Failed to reset password");

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiry = null;
            await _userManager.UpdateAsync(user);

            return (true, "Password reset successful");
        }

        public async Task<(bool success, string message)> VerifyEmailAsync(string token)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.EmailVerificationToken == token);

            if (user == null || user.EmailVerificationTokenExpiry <= DateTime.UtcNow)
                return (false, "Invalid or expired verification token");

            user.EmailConfirmed = true;
            user.EmailVerificationToken = null;
            user.EmailVerificationTokenExpiry = null;
            await _userManager.UpdateAsync(user);

            return (true, "Email verified successfully");
        }

        public async Task<Result> AssignRoleAsync(string userId, string role)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return new Result { success = false, message = "User not found" };
            }

            var result = await _userManager.AddToRoleAsync(user, role);
            if (result.Succeeded)
            {
                return new Result { success = true, message = "Role assigned successfully" };
            }

            return new Result { success = false, message = string.Join(", ", result.Errors.Select(e => e.Description)) };
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }
}
