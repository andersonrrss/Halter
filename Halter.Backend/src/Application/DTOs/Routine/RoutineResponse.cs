using GymApp.Application.DTOs;
using GymApp.Domain.Entities;

namespace GymApp.Application.DTOs;

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
