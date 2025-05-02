using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using Domain.Entities;
using Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<User?> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Username == username);
        }

        public async Task<bool> UserExistsAsync(string username)
        {
            return await _context.Users.AnyAsync(user => user.Username == username);
        }

        public async Task<OperationResult<User>> CreateUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return OperationResult<User>.Success(user);
        }

        public async Task<OperationResult<User>> LoginUserAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.Username == username);

            if (user == null)
                return OperationResult<User>.Failure("User not found");

            if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
                return OperationResult<User>.Failure("Invalid password");

            return OperationResult<User>.Success(user);
        }

        public async Task<OperationResult<List<User>>> GetAllAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return OperationResult<List<User>>.Success(users);
            }
            catch (Exception ex)
            {
                return OperationResult<List<User>>.Failure($"Error fetching users: {ex.Message}.");
            }
        }
    }
}
