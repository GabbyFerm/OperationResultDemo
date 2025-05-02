using Application.Common;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<OperationResult<string>>
    {
        public string Username { get; set; } = null!;

        public string Password { get; set; } = null!;

        public CreateUserCommand(string userName, string password)
        {
            Username = userName;
            Password = password;
        }
    }
}
