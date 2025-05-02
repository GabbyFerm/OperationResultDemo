using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Users.Queries.ListAllUsers
{
    public class ListAllUsersQuery : IRequest<OperationResult<List<UserDto>>>
    {
        // Could add extra logic here if needed
    }
}
