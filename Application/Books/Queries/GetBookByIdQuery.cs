using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Books.Queries
{
    public class GetBookByIdQuery : IRequest<OperationResult<BookDto>>
    {
        public int Id { get; set; }
        public GetBookByIdQuery(int id) => Id = id;
    }
}
