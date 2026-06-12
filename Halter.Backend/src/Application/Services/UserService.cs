using Halter.Application.Interfaces;
using Halter.Domain.Common;
using Halter.Application.DTOs;

namespace Halter.Application.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<Result<UserResponse>> GetUserInformationAsync(Guid userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);

        if(user is null)
            return Result<UserResponse>.NotFound();

        return Result<UserResponse>.Success(UserResponse.FromEntity(user));
    }
}
