using System.Collections.Generic;
using System.Threading.Tasks;
using Backend_Goalify.Core.Entities;
using Backend_Goalify.Core.Interfaces;

namespace Backend_Goalify.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public async Task<IEnumerable<Role>> GetAllRolesAsync()
        {
            return await _roleRepository.GetAllRolesAsync();
        }

        public async Task<Role> GetRoleByIdAsync(string id)
        {
            return await _roleRepository.GetRoleByIdAsync(id);
        }

        public async Task<Role> CreateRoleAsync(Role role)
        {
            return await _roleRepository.CreateRoleAsync(role);
        }

        public async Task UpdateRoleAsync(Role role)
        {
            await _roleRepository.UpdateRoleAsync(role);
        }

        public async Task DeleteRoleAsync(string id)
        {
            await _roleRepository.DeleteRoleAsync(id);
        }
    }
}