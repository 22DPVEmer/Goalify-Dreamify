using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Models;
using Backend_Goalify.Core.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;

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

        public async Task<(bool success, string token)> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return (false, null);

            user.LastLogin = DateTime.UtcNow;
            await _userManager.UpdateAsync(user);

            var token = _jwtService.GenerateToken(user);
            return (true, token);
        }

        public async Task<(bool success, string message)> RegisterAsync(RegisterModel model)
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

            return (true, "User created successfully");
        }

        public async Task<(bool success, string token, string message)> RefreshTokenAsync(string refreshToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken == refreshToken);
            
            if (user == null || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return (false, null, "Invalid or expired refresh token");

            var newToken = _jwtService.GenerateToken(user);
            var newRefreshToken = GenerateRefreshToken();
            
            user.RefreshToken = newRefreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
            await _userManager.UpdateAsync(user);

            return (true, newToken, null);
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

        public async Task<(bool success, string message)> AssignRoleAsync(string userId, string role)
        {
            if (!new[] { Roles.Admin, Roles.Moderator, Roles.Premium, Roles.Basic }
                .Contains(role))
                return (false, "Invalid role specified");

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return (false, "User not found");

            if (!await _roleManager.RoleExistsAsync(role))
                await _roleManager.CreateAsync(new IdentityRole(role));

            if (await _userManager.IsInRoleAsync(user, role))
                return (false, "User already has this role");

            var result = await _userManager.AddToRoleAsync(user, role);
            if (!result.Succeeded)
                return (false, "Failed to assign role");

            return (true, "Role assigned successfully");
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
