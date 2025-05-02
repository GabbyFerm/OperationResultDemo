using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Queries.GetAuthorById
{
    public class GetAuthorByIdQueryHandler : IRequestHandler<GetAuthorByIdQuery, OperationResult<AuthorDto>>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IMapper _mapper;

        public GetAuthorByIdQueryHandler(IGenericRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<AuthorDto>> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
        {
            // Try to find the author by ID
            var result = await _repository.GetByIdAsync(request.Id);

            // If not found or failed, return a failure result
            if (!result.IsSuccess || result.Data == null)
                return OperationResult<AuthorDto>.Failure(result.ErrorMessage ?? "Author not found");

            // Map the found Author to AuthorDto
            var mapped = _mapper.Map<AuthorDto>(result.Data);

            // Return success with the mapped result
            return OperationResult<AuthorDto>.Success(mapped);
        }
    }
}
