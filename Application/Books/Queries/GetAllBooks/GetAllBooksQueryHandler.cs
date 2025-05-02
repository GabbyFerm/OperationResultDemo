using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Books.Queries.GetAllBooks
{
    public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, OperationResult<IEnumerable<BookDto>>>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IMapper _mapper;

        public GetAllBooksQueryHandler(IGenericRepository<Book> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<IEnumerable<BookDto>>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
        {
            // Retrieve all books from repository
            var result = await _repository.GetAllAsync();

            // Return failure if fetching failed
            if (!result.IsSuccess)
                return OperationResult<IEnumerable<BookDto>>.Failure(result.ErrorMessage!);

            // Map Book entities to DTOs
            var dtoList = _mapper.Map<IEnumerable<BookDto>>(result.Data);

            // Return success with list of DTOs
            return OperationResult<IEnumerable<BookDto>>.Success(dtoList);
        }
    }
}
