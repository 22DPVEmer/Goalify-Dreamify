using Backend_Goalify.Core.Entities;

namespace Backend_Goalify.Core.Interfaces
{
    public interface ITagRepository : IRepository<Tag>
    {
        new Task AddAsync(Tag entity);
        Task<Tag?> FindByNameAsync(string name);
    }
}
 