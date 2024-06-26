using ContactManager.Interface;

namespace ContactManager.Service
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ContactManagerContext _dbContext;

        public UnitOfWork(ContactManagerContext context)
        {
            _dbContext = context;
        }

        public async Task<int> Save()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public IGenericRepository<T> GetRepository<T>() where T : class
        {
            return new GenericRepository<T>(_dbContext);
        }
    }
}
