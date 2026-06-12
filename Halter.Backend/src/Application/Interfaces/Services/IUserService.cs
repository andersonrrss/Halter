using Halter.Domain.Common;
using Halter.Application.DTOs;

namespace Halter.Application.Interfaces;

public interface IUserService
{    
    Task<Result<UserResponse>> GetUserInformationAsync(Guid userId);
}
