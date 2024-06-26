namespace ContactManager.Interface
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);

        Task<bool> Add(T entity);

        Task<bool> AddRange(IEnumerable<T> entities);

        Task<bool> Delete(int id);

        Task<bool> Upsert(T entity);
    }
}
