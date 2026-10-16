namespace BK.EventOps.Infrastructure.Interface
{
    public interface IDBRepository<T> where T : class
    {
        // Add new entity
        Task<T> AddAsync(T entity, CancellationToken cancellationToken=default);

        Task<int> SaveAsync(T entity, CancellationToken cancellationToken=default);

        Task<int> AddRangeAsync(List<T> entity, CancellationToken cancellationToken=default);

        Task<T> FetchDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken=default);

        // Get by Id
        Task<T> GetByIdAsync(long id, CancellationToken cancellationToken=default);

        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken=default);

        // Remove entity
        Task<int> RemoveAsync(T entity, CancellationToken cancellationToken=default);

        // Get all entities
        Task<IEnumerable<T>> GetAllAsync(bool include = false, CancellationToken cancellationToken = default);

        // Get all needed entities with filter
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

        // Get all needed entities with selected columns
        Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default);

        // Find entities based on a predicate (using Func<>)
        Task<IEnumerable<T>> FindListAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);

        Task<T> FindAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default);

        // Update entity
        Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default);

        Task<int> UpdateRangeAsync(List<T> entity, CancellationToken cancellationToken = default);

        // Delete entity
        Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default);

        Task<int> RemoveRangeAsync(List<T> entity, CancellationToken cancellationToken = default);

        Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default);

        Task<IEnumerable<TDTO>> ExecuteQueryAsync<TDTO>(string sQuery, object parameters);

        IQueryable<T> Query();
    }
}
