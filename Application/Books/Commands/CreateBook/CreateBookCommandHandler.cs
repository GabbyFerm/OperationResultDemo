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
            var entity = _mapper.Map<Book>(request);
            var result = await _repository.CreateAsync(entity);

            if (!result.IsSuccess)
                return OperationResult<BookDto>.Failure(result.ErrorMessage!);

            return OperationResult<BookDto>.Success(_mapper.Map<BookDto>(result.Data));
        }
    }

}
