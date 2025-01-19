using Backend_Goalify.Core.Entities;


namespace Backend_Goalify.Core.Interfaces
{

    public interface IRoleRepository : IRepository<Role>
    {
        Task<IEnumerable<Role>> GetAllRolesAsync();
        Task<Role> GetRoleByIdAsync(string id);
        Task<Role> CreateRoleAsync(Role role);
        Task UpdateRoleAsync(Role role);
        Task DeleteRoleAsync(string id);
    }
}