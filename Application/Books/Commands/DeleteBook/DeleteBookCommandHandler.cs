using Application.Common;
using Application.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Books.Commands.DeleteBook
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
            // Try to fetch the book by ID from the database
            var existing = await _repository.GetByIdAsync(request.Id);

            // If the book doesn't exist or an error occurred, return a failure
            if (!existing.IsSuccess || existing.Data == null)
                return OperationResult<bool>.Failure(existing.ErrorMessage ?? "Book not found");

            // Delete the found book and return the result
            return await _repository.DeleteAsync(existing.Data);
        }
    }
}
