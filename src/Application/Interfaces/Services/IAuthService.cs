using GymApp.Domain.Common;
using GymApp.Application.DTOs;

namespace GymApp.Application.Interfaces;

public interface IAuthService
{
    Task<Result<string>> TryLoginUserAsync(LoginRequest loginDTO);

    Task<Result<string>> TryRegisterUserAsync(RegisterRequest registerDTO);
}
