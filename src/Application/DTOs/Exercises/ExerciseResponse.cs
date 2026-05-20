using GymApp.Domain.Enums;
using GymApp.Domain.Entities;

namespace GymApp.Application.DTOs;

public record class ExerciseResponse
{
    public int Id { get; init; }
    public string Name { get; init; } = null!;
    public TimeConstraint TimeConstraint { get; init; }
    public MuscleGroup MuscleGroup { get; init; } = null!;

    public static ExerciseResponse FromEntity(Exercise exercise) => new()
    {
        Id = exercise.Id,
        Name = exercise.Name,
        TimeConstraint = exercise.TimeConstraint,
        MuscleGroup = exercise.MuscleGroup
    };
}
