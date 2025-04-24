using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Authors.Commands.DeleteAuthor
{
    public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, OperationResult<bool>>
    {
        private readonly IGenericRepository<Author> _repository;

        public DeleteAuthorCommandHandler(IGenericRepository<Author> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteAuthorCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (!existing.IsSuccess || existing.Data == null)
                return OperationResult<bool>.Failure(existing.ErrorMessage ?? "Author not found");

            return await _repository.DeleteAsync(existing.Data);
        }
    }

}
