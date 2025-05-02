using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Queries.GetAllAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, OperationResult<IEnumerable<AuthorDto>>>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IMapper _mapper;

        public GetAllAuthorsQueryHandler(IGenericRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<AuthorDto>>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
        {
            // Get all authors from the repository
            var result = await _repository.GetAllAsync();

            // Return failure result if something went wrong
            if (!result.IsSuccess)
                return OperationResult<IEnumerable<AuthorDto>>.Failure(result.ErrorMessage ?? "An error occurred while retrieving authors.");

            // Map the list of Author entities to a list of AuthorDto
            var mapped = _mapper.Map<IEnumerable<AuthorDto>>(result.Data);

            // Return the list wrapped in a success result
            return OperationResult<IEnumerable<AuthorDto>>.Success(mapped);
        }
    }
}
