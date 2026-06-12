using Halter.Domain.Entities;

namespace Halter.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);

    Task<User?> GetUserByEmailAsync(string email);

    Task AddAsync(User user);
}
