using Dapper;

namespace RebarPro.Infrastructure.Repositories
{
    public class DBRepository<T> : IDBRepository<T> where T : class
    {
        private readonly BkEventOpsDbContext _context;  // Use DbContext directly
        private readonly DbSet<T> _dbSet;

        public DBRepository(BkEventOpsDbContext context, CancellationToken cancellationToken = default)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _dbSet = _context.Set<T>();
        }

        // Add new entity
        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(entity);

            await _dbSet.AddAsync(entity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return entity;
        }

        public async Task<int> SaveAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            await _dbSet.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> AddRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            _dbSet.AddRange(entities);

            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<T> FetchDefaultAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstOrDefaultAsync(predicate, cancellationToken) ?? throw new KeyNotFoundException();
        }

        // Get by Id
        public async Task<T> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException();
        }

        public async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException();
        }

        // Get all entities
        public async Task<IEnumerable<T>> GetAllAsync(bool include = false, CancellationToken cancellationToken = default)
        {
            if (include)
            {
                return await _dbSet.IgnoreQueryFilters().ToListAsync(cancellationToken);
            }
            else
            {
                // As No Tracking
                return await _dbSet.AsNoTracking().ToListAsync(cancellationToken);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="predicate"> Predicate to filter results</param>
        /// <param name="selector">Selector to map the result </param>
        /// <returns></returns>
        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, bool>> predicate, Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(predicate)  // Apply WHERE condition
                .Select(selector)   // Select the projection
                .ToListAsync(cancellationToken);     // Execute query and return results
        }

        public async Task<IEnumerable<TResult>> GetAllAsync<TResult>(Expression<Func<T, TResult>> selector, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AsNoTracking() // Improves performance by not tracking the entities
                .Select(selector)
                .ToListAsync(cancellationToken);
        }

        // Find entities based on a predicate
        public async Task<IEnumerable<T>> FindListAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => _dbSet.AsNoTracking().Where(predicate).ToList());
        }

        // Find entities based on a predicate
        public async Task<T> FindAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => _dbSet.AsNoTracking().FirstOrDefault(predicate)) ?? throw new KeyNotFoundException();
        }

        // Update entity

        public async Task<int> UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Attach(entity);
            var entry = _context.Entry(entity);
            entry.State = EntityState.Modified;
            foreach (var property in entry.Properties)
            {
                // HasDefaultValue on flags (e.g. IsActive) also marks ValueGenerated.OnAdd.
                // Skipping every OnAdd property prevents activate/deactivate from saving.
                // Only skip non-boolean insert-generated columns (PKs, CreatedAt, etc.).
                if (property.Metadata.ValueGenerated != Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.OnAdd)
                {
                    continue;
                }
                var clrType = Nullable.GetUnderlyingType(property.Metadata.ClrType) ?? property.Metadata.ClrType;
                if (clrType != typeof(bool))
                {
                    property.IsModified = false;
                }
            }
            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entities);

            _dbSet.UpdateRange(entities);
            return await _context.SaveChangesAsync();
        }

        // Remove entity
        public async Task<int> RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            ArgumentNullException.ThrowIfNull(entity);
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id, CancellationToken cancellationToken = default)
        {
            T entity = await _dbSet.FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Entity not found.");
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> RemoveRangeAsync(List<T> entities, CancellationToken cancellationToken = default)
        {
            // Check if the entities list is null or empty to avoid unnecessary database operations
            if (entities == null || entities.Count == 0)
            {
                throw new ArgumentNullException(nameof(entities), "The entities list cannot be null or empty.");
            }

            // Remove the entities from the DbSet
            _dbSet.RemoveRange(entities);

            // Save changes asynchronously and return the number of affected rows
            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            T entity = await _dbSet.FindAsync(id, cancellationToken) ?? throw new KeyNotFoundException("Entity not found.");
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<TDTO>> ExecuteQueryAsync<TDTO>(string sQuery, object parameters)
        {
            var result = new List<TDTO>();
            using (var dbConnection = _context.Database.GetDbConnection())
            {
                await dbConnection.OpenAsync();
                // Execute the Query using Dapper
                var entities = await dbConnection.QueryAsync<TDTO>(sQuery, parameters, commandType: CommandType.Text);
                result = entities.AsList(); // Convert to List<TDTO>
            }
            return result;
        }

        public IQueryable<T> Query()
        {
            return _dbSet.AsNoTracking(); //Ensures better performance for read-only queries
        }
    }
}