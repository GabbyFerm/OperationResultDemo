using Application.Common;
using Application.DTOs;
using Application.Interfaces;
using AutoMapper;
using MediatR;

namespace Application.Users.Queries.ListAllUsers
{
    public class ListAllUsersQueryHandler : IRequestHandler<ListAllUsersQuery, OperationResult<List<UserDto>>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ListAllUsersQueryHandler(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<OperationResult<List<UserDto>>> Handle(ListAllUsersQuery request, CancellationToken cancellationToken)
        {
            var result = await _userRepository.GetAllAsync();

            if (!result.IsSuccess || result.Data == null)
                return OperationResult<List<UserDto>>.Failure(result.ErrorMessage ?? "No users found");

            var userDto = _mapper.Map<List<UserDto>>(result.Data);

            return OperationResult<List<UserDto>>.Success(userDto);
        }
    }
}