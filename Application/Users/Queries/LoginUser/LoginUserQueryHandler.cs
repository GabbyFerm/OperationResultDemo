using Application.Common;
using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries.LoginUser
{
    public class LoginUserQueryHandler : IRequestHandler<LoginUserQuery, OperationResult<string>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IJwtGenerator _jwtGenerator;

        public LoginUserQueryHandler(IUserRepository userRepository, IJwtGenerator jwtGenerator)
        {
            _userRepository = userRepository;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<OperationResult<string>> Handle(LoginUserQuery request, CancellationToken cancellationToken)
        {
            // Look up the user by username
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            // Check if user exists and password is correct
            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return OperationResult<string>.Failure("Invalid credentials");

            // Create a JWT token for the user
            var token = _jwtGenerator.GenerateToken(user);

            // Return the token inside a success result
            return OperationResult<string>.Success(token);
        }
    }
}