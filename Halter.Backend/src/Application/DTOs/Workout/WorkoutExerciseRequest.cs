using GymApp.Domain.Enums;

namespace GymApp.Application.DTOs;

public record class WorkoutExerciseRequest
{
    public int ExerciseId { get; init; }
    public int Sets { get; init; }
    public ExerciseType ExerciseType { get; init; }
    public IList<int> Values { get; init; } = [];
    public int? IsometricHoldSeconds { get; init; }
}
