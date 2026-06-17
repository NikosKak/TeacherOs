using Microsoft.EntityFrameworkCore;
using TeacherOs.Data;

namespace TeacherOs.Repositories
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly SchoolOsContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(SchoolOsContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }
        public virtual async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }
        public virtual async Task AddRangeAsync(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }
        public virtual Task UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            _context.Entry(entity).State = EntityState.Modified;
            return Task.CompletedTask;
        }
        public virtual async Task<bool> DeleteAsync(int id)
        {
            T? existingEntity = await _dbSet.FindAsync(id);
            if (existingEntity == null)
            {
                return false;
            }
            _dbSet.Remove(existingEntity);
            return true;
        }
        public virtual async Task<T?> GetAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public virtual async Task<int> GetCountAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
