using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Books.Commands
{
    public class CreateBookCommand : IRequest<OperationResult<BookDto>>
    {
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }
    }
}
