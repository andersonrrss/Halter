using GymApp.Domain.Entities;

namespace GymApp.Application;

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
