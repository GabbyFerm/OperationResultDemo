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
            var userExists = await _userRepository.UserExistsAsync(request.Username);
            if (userExists)
                return OperationResult<string>.Failure("User is already registered.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Username = request.Username,
                Password = hashedPassword,
                Role = "User" // Default
            };

            await _userRepository.CreateUserAsync(user);

            var token = _jwtGenerator.GenerateToken(user);
            return OperationResult<string>.Success(token);

        }
    }
}
