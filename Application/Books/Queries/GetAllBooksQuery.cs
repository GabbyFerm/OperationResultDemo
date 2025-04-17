using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Books.Queries
{
    public class GetAllBooksQuery : IRequest<OperationResult<IEnumerable<BookDto>>>
    {
    }
}
