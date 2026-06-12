using Halter.Application.DTOs;
using Halter.Domain.Entities;

namespace Halter.Application.DTOs;

public record class RoutineResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public IEnumerable<WorkoutResponse> Workouts { get; init; } = [];

    public static RoutineResponse FromEntity(Routine routine) => new()
    {
        Id = routine.Id,
        Name = routine.Name,
        Workouts = routine.Workouts.Select(w => WorkoutResponse.FromEntity(w)).ToList()
    };
}
