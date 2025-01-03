using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Core.Models;
using Microsoft.Extensions.Logging;

namespace Backend_Goalify.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, IMapper mapper, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<UserModel> GetUserProfileAsync(string email)
        {
            var user = await _userRepository.GetByUsernameAsync(email);
            if (user == null)
                throw new KeyNotFoundException($"User not found with email: {email}");

            return _mapper.Map<UserModel>(user);
        }

        public async Task<UserModel> GetProfileAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByUsernameAsync(email);
                if (user == null)
                {
                    _logger.LogWarning($"User not found for email: {email}");
                    return null;
                }

                

                return new UserModel
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    IsActive = user.IsActive,
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
            var users = await _userRepository.GetAllAsync();
            return _mapper.Map<IEnumerable<UserModel>>(users);
        }

        public async Task<UserModel> UpdateUserAsync(UserModel userModel)
        {
            var userEntity = _mapper.Map<ApplicationUser>(userModel);
            await _userRepository.UpdateAsync(userEntity);
            return _mapper.Map<UserModel>(userEntity);
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user != null)
            {
                var userId = user.Id;
                await _userRepository.DeleteAsync(userId);
            }
        }
    }
}