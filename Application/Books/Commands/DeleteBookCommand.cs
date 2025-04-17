using Application.Common;
using MediatR;

namespace Application.Books.Commands
{
    public class DeleteBookCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
        public DeleteBookCommand(int id) => Id = id;
    }
}
