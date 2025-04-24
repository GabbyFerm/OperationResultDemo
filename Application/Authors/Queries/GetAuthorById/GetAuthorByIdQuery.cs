using Application.Common;
using Application.DTOs;
using MediatR;

namespace Application.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQuery : IRequest<OperationResult<AuthorDto>>
    {
        public int Id { get; set; }
        public GetAuthorByIdQuery(int id)
        {
            Id = id;
        }
    }
}
