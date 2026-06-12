using Halter.Domain.Entities;

namespace Halter.Application.DTOs;

public record class WorkoutResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;

    public static WorkoutResponse FromEntity(Workout workout) => new()
    {
        Id = workout.Id,
        Name = workout.Name,
    };
}
