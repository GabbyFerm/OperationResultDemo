using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Commands.CreateAuthor
{
    public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, OperationResult<AuthorDto>>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IMapper _mapper;

        public CreateAuthorCommandHandler(IGenericRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<AuthorDto>> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
        {
            // Map the incoming command (DTO) to the Author entity
            var entity = _mapper.Map<Author>(request);

            // Save the new author to the database using the generic repository
            var result = await _repository.CreateAsync(entity);

            // If something went wrong during creation, return a failure result
            if (!result.IsSuccess)
                return OperationResult<AuthorDto>.Failure(result.ErrorMessage!);

            // Map the saved entity back to AuthorDto for the response
            var dto = _mapper.Map<AuthorDto>(result.Data);

            // Return success result with the created author
            return OperationResult<AuthorDto>.Success(dto);
        }
    }
}
