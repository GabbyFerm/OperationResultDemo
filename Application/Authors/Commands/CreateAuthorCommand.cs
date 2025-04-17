using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Authors.Commands
{
    public class CreateAuthorCommand : IRequest<OperationResult<AuthorDto>>
    {
        public string Name { get; set; } = null!;
    }
}
