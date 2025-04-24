using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Books.Queries.GetBookById
{
    public class GetBookByIdQueryHandler : IRequestHandler<GetBookByIdQuery, OperationResult<BookDto>>
    {
        private readonly IGenericRepository<Book> _repository;
        private readonly IMapper _mapper;

        public GetBookByIdQueryHandler(IGenericRepository<Book> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult<BookDto>> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            //// Simulate a crash
            //throw new Exception("Something went wrong while retrieving the book!");


            var result = await _repository.GetByIdAsync(request.Id);

            if (!result.IsSuccess || result.Data == null)
                return OperationResult<BookDto>.Failure(result.ErrorMessage ?? "Book not found");

            var dto = _mapper.Map<BookDto>(result.Data);
            return OperationResult<BookDto>.Success(dto);
        }
    }

}
