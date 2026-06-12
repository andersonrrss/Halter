using Halter.Domain.Entities;
using Halter.Domain.Enums;

namespace Halter.Application;

public record class ExerciseHistoryEntryResponse
{
    public Guid Id { get; init; }
    public ExerciseType ExerciseType { get; init; }
    public DateTime CompletedAt { get; init; }
    public int ValueAchieved { get; init; }
    public int SetsCompleted { get; init; }
    public double? MaxWeight { get; init; }

    public static ExerciseHistoryEntryResponse FromEntity(ExerciseEntry entry) => new()
    {
        Id = entry.Id,
        ExerciseType = entry.ExerciseType,
        CompletedAt = entry.CompletedAt,
        ValueAchieved = entry.ValueAchieved,
        SetsCompleted = entry.SetsCompleted,
        MaxWeight = entry.MaxWeight
    };
}
