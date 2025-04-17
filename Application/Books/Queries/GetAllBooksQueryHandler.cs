using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using MediatR;

namespace Application.Books.Queries
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
            var result = await _repository.GetAllAsync();

            if (!result.IsSuccess)
                return OperationResult<IEnumerable<BookDto>>.Failure(result.ErrorMessage!);

            var dtoList = _mapper.Map<IEnumerable<BookDto>>(result.Data);
            return OperationResult<IEnumerable<BookDto>>.Success(dtoList);
        }
    }


}
