using GymApp.Domain.Entities;

namespace GymApp.Application.DTOs;

public record class FinishedWorkoutSessionResponse
{
    public Guid Id { get; init; }
    public Guid? WorkoutId { get; init; }
    public DateTime StartedAt { get; init; }
    public DateTime FinishedAt { get; init; }
    public int TotalPausedSeconds { get; init; }
    public TimeSpan TotalTime { get; init; }
    public IEnumerable<ExerciseEntryResponse> ExerciseEntries { get; init; } = [];

    public static FinishedWorkoutSessionResponse FromEntity(WorkoutSession workoutSession) => new()
    {
        Id = workoutSession.Id,
        WorkoutId = workoutSession.WorkoutId,
        StartedAt = workoutSession.StartedAt,
        FinishedAt = workoutSession.FinishedAt!.Value,
        TotalPausedSeconds = workoutSession.TotalPausedSeconds,
        TotalTime = workoutSession.TotalTime,
        ExerciseEntries = workoutSession.ExerciseEntries
            .Select(ee => ExerciseEntryResponse.FromEntity(ee))
    };
}
