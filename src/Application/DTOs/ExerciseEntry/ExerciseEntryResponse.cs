using GymApp.Domain.Entities;
using GymApp.Domain.Enums;

namespace GymApp.Application.DTOs;

public record class ExerciseEntryResponse
{
    public Guid Id { get; init; }
    public ExerciseSimpleResponse Exercise { get; init; } = null!;
    public Guid WorkoutSessionId { get; init; }
    public ExerciseType ExerciseType { get; init; }
    public DateTime CompletedAt { get; init; }
    public int ValueAchieved { get; init; }
    public int SetsCompleted { get; init; }
    public double? MaxWeight { get; init; }

    public static ExerciseEntryResponse FromEntity(ExerciseEntry entry, Exercise? exercise = null) => new()
    {
        Id = entry.Id,
        Exercise = ExerciseSimpleResponse.FromEntity(
            exercise is null ? entry.Exercise : exercise
        ),
        WorkoutSessionId = entry.Id,
        ExerciseType = entry.ExerciseType,
        CompletedAt = entry.CompletedAt,
        ValueAchieved = entry.ValueAchieved,
        SetsCompleted = entry.SetsCompleted,
        MaxWeight = entry.MaxWeight
    };
}
