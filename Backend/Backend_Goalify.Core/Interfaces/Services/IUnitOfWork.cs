using Backend_Goalify.Core.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Backend_Goalify.Core.Interfaces
{
    public interface IUnitOfWork
    {
        IGoalEntryRepository GoalEntryRepository { get; }
        
        Task<bool> SaveAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<bool> CommitAsync(IDbContextTransaction transaction);
        Task RollbackAsync(IDbContextTransaction transaction);
    }
}
