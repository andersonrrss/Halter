using System.Text.Json.Serialization;
using GymApp.Domain.Entities;
using GymApp.Domain.Enums;

namespace GymApp.Application.DTOs;

public record class WorkoutExerciseResponse
{
    public Guid Id { get; init; }
    
    public int ExerciseId { get; init; }
    public string ExerciseName { get; init; } = null!;
    
    public ExerciseType ExerciseType { get; init; }
    public IList<int> Values { get; init; } = [];
    public int Sets { get; init; }
    
    public int? IsometricHoldSeconds { get; init; }

    public static WorkoutExerciseResponse FromEntity(WorkoutExercise workoutExercise, string exerciseName = "") => new()
    {
        Id = workoutExercise.Id,
        Sets = workoutExercise.Sets,
        ExerciseId = workoutExercise.ExerciseId,
        ExerciseName = workoutExercise.Exercise is null 
            ? exerciseName
            : workoutExercise.Exercise.Name,
        ExerciseType = workoutExercise.ExerciseType,
        Values = workoutExercise.Values,
        IsometricHoldSeconds = workoutExercise.IsometricHoldSeconds
    };
}
