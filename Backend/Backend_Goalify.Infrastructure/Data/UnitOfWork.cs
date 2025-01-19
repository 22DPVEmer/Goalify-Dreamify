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
            IGoalEntryRepository goalEntryRepository,
            ITagRepository tagRepository,
            IUserRepository userRepository,
            ICategoryRepository categoryRepository){

            _context = context;
            GoalEntryRepository = goalEntryRepository;
            TagRepository = tagRepository;
            UserRepository = userRepository;
            CategoryRepository = categoryRepository;

        }

        
    public IGoalEntryRepository GoalEntryRepository { get; }
    public ITagRepository TagRepository { get; }
    public IUserRepository UserRepository { get; }
    public ICategoryRepository CategoryRepository { get; }


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