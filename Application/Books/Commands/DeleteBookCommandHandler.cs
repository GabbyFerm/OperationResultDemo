using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Books.Commands
{
    public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, OperationResult<bool>>
    {
        private readonly IGenericRepository<Book> _repository;

        public DeleteBookCommandHandler(IGenericRepository<Book> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResult<bool>> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            var existing = await _repository.GetByIdAsync(request.Id);
            if (!existing.IsSuccess || existing.Data == null)
                return OperationResult<bool>.Failure(existing.ErrorMessage ?? "Book not found");

            return await _repository.DeleteAsync(existing.Data);
        }
    }

}
