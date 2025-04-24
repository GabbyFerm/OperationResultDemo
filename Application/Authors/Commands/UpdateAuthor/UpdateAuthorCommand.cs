using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommand : IRequest<OperationResult<AuthorDto>>
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
