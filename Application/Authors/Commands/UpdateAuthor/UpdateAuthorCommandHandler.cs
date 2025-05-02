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
            // Try to find the existing author by ID
            var existing = await _repository.GetByIdAsync(request.Id);

            // If the author is not found, return a failure result
            if (!existing.IsSuccess || existing.Data == null)
                return OperationResult<AuthorDto>.Failure(existing.ErrorMessage ?? "Author not found");

            // Update the author with the new name
            var author = existing.Data;
            author.Name = request.Name;

            // Save the changes
            var updated = await _repository.UpdateAsync(author);

            // Map the updated entity back to a DTO and return success
            return OperationResult<AuthorDto>.Success(_mapper.Map<AuthorDto>(updated.Data));
        }
    }
}
