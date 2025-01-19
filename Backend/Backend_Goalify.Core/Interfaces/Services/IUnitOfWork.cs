using Backend_Goalify.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend_Goalify.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGoalEntryRepository GoalEntryRepository { get; }
        ITagRepository TagRepository { get; }
        IUserRepository UserRepository { get; }

        ICategoryRepository CategoryRepository { get; }
        
        
        
        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
