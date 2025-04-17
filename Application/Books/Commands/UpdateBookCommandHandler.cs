using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Books.Commands
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
            var existingResult = await _repository.GetByIdAsync(request.Id);
            if (!existingResult.IsSuccess || existingResult.Data == null)
                return OperationResult<BookDto>.Failure(existingResult.ErrorMessage ?? "Book not found");

            var book = existingResult.Data;
            book.Title = request.Title;
            book.AuthorId = request.AuthorId;

            var updateResult = await _repository.UpdateAsync(book);
            return OperationResult<BookDto>.Success(_mapper.Map<BookDto>(updateResult.Data));
        }
    }

}
