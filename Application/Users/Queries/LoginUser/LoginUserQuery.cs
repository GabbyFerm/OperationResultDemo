using Application.Common;
using MediatR;

namespace Application.Users.Queries.LoginUser
{
    public class LoginUserQuery : IRequest<OperationResult<string>>
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public LoginUserQuery(string userName, string password)
        {
            Username = userName;
            Password = password;
        }
    }
}
