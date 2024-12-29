using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Backend_Goalify.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(UserManager<ApplicationUser> userManager, IMapper mapper, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserModel> GetUserProfileAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"User not found with email: {email}");

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> GetProfileAsync(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {email}");
                    return null;
                }

                var roles = await _userManager.GetRolesAsync(user);

                return new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
                    IsAdmin = roles.Contains("Admin")
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error getting profile for user {email}");
                throw;
            }
        }

        public async Task<IEnumerable<UserModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel userModel)
        {
            var userEntity = _mapper.Map<ApplicationUser>(userModel);
            await _userManager.UpdateAsync(userEntity);
            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                await _userManager.DeleteAsync(user);
            }
        }
    }
}