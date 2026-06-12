using Halter.Domain.Entities;

namespace Halter.Application.DTOs;

public record class WorkoutWithExercisesResponse
{
    public Guid Id { get; init; }
    public string Name { get; init; } = null!;
    public IEnumerable<WorkoutExerciseResponse> WorkoutExercises { get; init; } = null!;

    public static WorkoutWithExercisesResponse FromEntity(Workout workout, IEnumerable<WorkoutExercise> workoutExercises) => new()
    {
        Id = workout.Id,
        Name = workout.Name,
        WorkoutExercises = workoutExercises.Select(e => WorkoutExerciseResponse.FromEntity(e)) ?? []
    };
}
