using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Books.Commands.UpdateBook
{
    public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult<BookDto>>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IMapper _mapper;

        public UpdateBookCommandHandler(IGenericRepository<Book> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<BookDto>> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            // Try to find the book by ID
            var existingResult = await _repository.GetByIdAsync(request.Id);

            // If not found or failed, return a failure response
            if (!existingResult.IsSuccess || existingResult.Data == null)
                return OperationResult<BookDto>.Failure(existingResult.ErrorMessage ?? "Book not found");

            // Update the book entity with new values from the request
            var book = existingResult.Data;
            book.Title = request.Title;
            book.AuthorId = request.AuthorId;

            // Save changes via the repository
            var updateResult = await _repository.UpdateAsync(book);

            // Return the updated book as a DTO inside an OperationResult
            return OperationResult<BookDto>.Success(_mapper.Map<BookDto>(updateResult.Data));
        }
    }
}
