using Application.Common;
using MediatR;

namespace Application.Books.Commands.DeleteBook
{
    public class DeleteBookCommand : IRequest<OperationResult<bool>>
    {
        public int Id { get; set; }
        public DeleteBookCommand(int id) => Id = id;
    }
}
