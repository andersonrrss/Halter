using Halter.Domain.Entities;

namespace Halter.Application.DTOs;

public record class UnfinishedWorkoutSessionResponse
{
    public Guid Id { get; init; }
    public Guid? WorkoutId { get; init; }
    public DateTime StartedAt { get; init; }
    public DateTime? PausedAt { get; init; }
    public int TotalPausedSeconds { get; init; }
    public IEnumerable<ExerciseEntryResponse> ExerciseEntries { get; init; } = [];

    public static UnfinishedWorkoutSessionResponse FromEntity(WorkoutSession workoutSession) => new()
    {
        Id = workoutSession.Id,
        WorkoutId = workoutSession.WorkoutId,
        StartedAt = workoutSession.StartedAt,
        PausedAt = workoutSession.PausedAt,
        TotalPausedSeconds = workoutSession.TotalPausedSeconds,
        ExerciseEntries = workoutSession.ExercisesEntries
            .Select(ee => ExerciseEntryResponse.FromEntity(ee))
    };
}
