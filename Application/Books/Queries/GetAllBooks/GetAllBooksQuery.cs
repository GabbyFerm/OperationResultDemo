using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQuery : IRequest<OperationResult<IEnumerable<BookDto>>>
    {
    }
}
