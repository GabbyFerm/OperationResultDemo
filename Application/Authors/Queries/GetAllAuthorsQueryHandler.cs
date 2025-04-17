using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Queries
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
            var result = await _repository.GetAllAsync();
            if (!result.IsSuccess)
                return OperationResult<IEnumerable<AuthorDto>>.Failure(result.ErrorMessage ?? "An error occurred while retrieving authors.");

            var mapped = _mapper.Map<IEnumerable<AuthorDto>>(result.Data);
            return OperationResult<IEnumerable<AuthorDto>>.Success(mapped);
        }
    }
}
