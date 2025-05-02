using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Books.Commands.CreateBook
{
    public class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult<BookDto>>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IMapper _mapper;

        public CreateBookCommandHandler(IGenericRepository<Book> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<BookDto>> Handle(CreateBookCommand request, CancellationToken cancellationToken)
        {
            // Map the incoming command to a Book entity
            var entity = _mapper.Map<Book>(request);

            // Try to save the new Book to the database
            var result = await _repository.CreateAsync(entity);

            // If the save operation failed, return a failure result
            if (!result.IsSuccess)
                return OperationResult<BookDto>.Failure(result.ErrorMessage!);

            // Map the saved entity to a BookDto and return a success result
            return OperationResult<BookDto>.Success(_mapper.Map<BookDto>(result.Data));
        }
    }

}
