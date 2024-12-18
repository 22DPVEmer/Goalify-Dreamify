using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Core.Models;

namespace Backend_Goalify.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserModel> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return _mapper.Map<UserModel>(user);
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
            await _userRepository.DeleteAsync(id);
        }
    }
}