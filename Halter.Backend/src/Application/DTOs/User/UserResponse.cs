using Halter.Domain.Entities;

namespace Halter.Application.DTOs;

public record class UserResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public string Email { get; init; } = null!;

    public static UserResponse FromEntity(User user) => new()
    {
        Id = user.Id,
        Name = user.Name,
        Email = user.Email,
    };
}
