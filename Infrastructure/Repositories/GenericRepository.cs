using Application.Common;
using Application.Interfaces;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        {
            var list = await _dbSet.ToListAsync();
            return OperationResult<IEnumerable<T>>.Success(list);
        }

        //// Raw sql to compare time between link and raw SQL
        //public async Task<OperationResult<IEnumerable<T>>> GetAllAsync()
        //{
        //    var list = await _dbSet
        //        .FromSqlRaw($"SELECT * FROM [{typeof(T).Name}s]")
        //        .ToListAsync();

        //    return OperationResult<IEnumerable<T>>.Success(list);
        //}

        public async Task<OperationResult<T?>> GetByIdAsync(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            return entity != null
                ? OperationResult<T?>.Success(entity)
                : OperationResult<T?>.Failure("Entity not found");
        }

        public async Task<OperationResult<T>> CreateAsync(T entity)
        {
            try
            {
                await _dbSet.AddAsync(entity);
                await _context.SaveChangesAsync();
                return OperationResult<T>.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResult<T>.Failure($"Error creating entity: {ex.Message}");
            }
        }

        public async Task<OperationResult<T>> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync();
            return OperationResult<T>.Success(entity);            
        }

        public async Task<OperationResult<bool>> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
            return OperationResult<bool>.Success(true);
        }
    }
}
