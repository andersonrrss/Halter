using Halter.Domain.Entities;

namespace Halter.Application;

public record class ExerciseSimpleResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;

    public static ExerciseSimpleResponse FromEntity(Exercise exercise) => new()
    {
        Id = exercise.Id,
        Name = exercise.Name
    };
}
