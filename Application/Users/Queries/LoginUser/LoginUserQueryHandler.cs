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
            var user = await _userRepository.GetByUsernameAsync(request.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
                return OperationResult<string>.Failure("Invalid credentials");

            var token = _jwtGenerator.GenerateToken(user);
            return OperationResult<string>.Success(token);
        }
    }
}