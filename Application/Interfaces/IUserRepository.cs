using Application.Common;
using Domain.Entities;

namespace Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> UserExistsAsync(string username);
        Task<OperationResult<User>> CreateUserAsync(User user);
        Task<OperationResult<User>> LoginUserAsync(string username, string password);
        Task<OperationResult<List<User>>> GetAllAsync();
    }
}
