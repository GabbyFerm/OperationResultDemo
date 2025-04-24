using Application.Common;
using MediatR;

namespace Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }

        public DeleteAuthorCommand(int id)
        {
            Id = id;
        }
    }
}
