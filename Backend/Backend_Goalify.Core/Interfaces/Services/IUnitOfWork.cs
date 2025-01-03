using Backend_Goalify.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend_Goalify.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGoalEntryRepository GoalEntryRepository { get; }
        ITagRepository TagRepository { get; }
        
        
        
        Task SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
