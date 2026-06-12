using Halter.Domain.Common;
using Halter.Application.DTOs;

namespace Halter.Application.Interfaces;

public interface IAuthService
{
    Task<Result<string>> TryLoginUserAsync(LoginRequest loginDTO);

    Task<Result<string>> TryRegisterUserAsync(RegisterRequest registerDTO);
}
