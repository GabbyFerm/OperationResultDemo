using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtGenerator _jwtGenerator;

        public CreateUserCommandHandler(IUserRepository userRepository, IJwtGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<OperationResult<string>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if username already exists
            var userExists = await _userRepository.UserExistsAsync(request.Username);

            // Return error if username is taken
            if (userExists)
                return OperationResult<string>.Failure("User is already registered.");

            // Secure the password
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                Role = "User" // Assign default role
            };

            // Save user to database
            await _userRepository.CreateUserAsync(user);

            // Generate JWT token for immediate login
            var token = _jwtGenerator.GenerateToken(user);

            // Return token as success response
            return OperationResult<string>.Success(token);
        }
    }
}
