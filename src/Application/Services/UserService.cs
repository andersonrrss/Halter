using GymApp.Application.Interfaces;
using GymApp.Domain.Common;
using GymApp.Application.DTOs;

namespace GymApp.Application.Services;

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
            return Result<UserResponse>.NotFound("Usuário não encontrado");

        return Result<UserResponse>.Success(UserResponse.FromEntity(user));
    }
}
