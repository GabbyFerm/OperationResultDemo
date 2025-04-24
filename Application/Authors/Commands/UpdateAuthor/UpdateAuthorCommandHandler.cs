using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Commands.UpdateAuthor
{
    public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, OperationResult<AuthorDto>>
    {
        private readonly IGenericRepository<Author> _repository;
        private readonly IMapper _mapper;

        public UpdateAuthorCommandHandler(IGenericRepository<Author> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<AuthorDto>> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (!existing.IsSuccess || existing.Data == null)
                return OperationResult<AuthorDto>.Failure(existing.ErrorMessage ?? "Author not found");

            var author = existing.Data;
            author.Name = request.Name;

            var updated = await _repository.UpdateAsync(author);
            return OperationResult<AuthorDto>.Success(_mapper.Map<AuthorDto>(updated.Data));
        }
    }

}
