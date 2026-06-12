using Halter.Domain.Entities;

namespace Halter.Application.DTOs;

public record class ExerciseProgressResponse
{
    public ExerciseHistoryEntryResponse PersonalRecord { get; init; } = null!;
    public IEnumerable<ExerciseHistoryEntryResponse> History { get; init; } = null!;

    public static ExerciseProgressResponse FromEntity(ExerciseEntry personalRecord, IEnumerable<ExerciseEntry> history) => new()
    {
        PersonalRecord = ExerciseHistoryEntryResponse.FromEntity(personalRecord),
        History = history.Select(ee => 
            ExerciseHistoryEntryResponse.FromEntity(ee))
    };
}
