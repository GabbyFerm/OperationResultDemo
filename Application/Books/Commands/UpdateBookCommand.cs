using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Books.Commands
{
    public class UpdateBookCommand : IRequest<OperationResult<BookDto>>
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int AuthorId { get; set; }
    }
}
