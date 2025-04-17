using Application.Common;

namespace Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<OperationResult<IEnumerable<T>>> GetAllAsync();
        Task<OperationResult<T?>> GetByIdAsync(int id);
        Task<OperationResult<T>> CreateAsync(T entity);
        Task<OperationResult<T>> UpdateAsync(T entity);
        Task<OperationResult<bool>> DeleteAsync(T entity);
    }
}
