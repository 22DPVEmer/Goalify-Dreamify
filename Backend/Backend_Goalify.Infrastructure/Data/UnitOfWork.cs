using Backend_Goalify.Core.Interfaces;
using Backend_Goalify.Infrastructure.Data;
using Microsoft.EntityFrameworkCore.Storage;
using System.Threading.Tasks;

namespace Backend_Goalify.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        public UnitOfWork(
            ApplicationDbContext context,
            IGoalEntryRepository goalEntryRepository){

            _context = context;
            GoalEntryRepository = goalEntryRepository;
        }
        
    public IGoalEntryRepository GoalEntryRepository { get; }

    public Task SaveAsync()
    {
        return _context.SaveChangesAsync();
    }

    public async Task<IDbContextTransaction> BeginTransactionAsync()
    {
        return await _context.Database.BeginTransactionAsync();
    }

}
}