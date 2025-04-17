using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Commands
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
            var entity = _mapper.Map<Author>(request);
            var result = await _repository.CreateAsync(entity);

            if (!result.IsSuccess)
                return OperationResult<AuthorDto>.Failure(result.ErrorMessage!);

            var dto = _mapper.Map<AuthorDto>(result.Data);
            return OperationResult<AuthorDto>.Success(dto);
        }
    }
}
