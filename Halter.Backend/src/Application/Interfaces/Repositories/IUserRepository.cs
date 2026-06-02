using GymApp.Domain.Entities;

namespace GymApp.Application.Interfaces;

public interface IUserRepository
{
    Task<User?> GetUserByIdAsync(Guid id);

    Task<User?> GetUserByEmailAsync(string email);

    Task AddAsync(User user);
}
