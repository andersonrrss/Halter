using System.Text.Json.Serialization;
using GymApp.Domain.Enums;

namespace GymApp.Domain.Entities;

public class Exercise
{
    public Exercise () {}

    [JsonConstructor]
    public Exercise(int id, string name, int muscleGroupId)
    {
        Id = id;
        Name = name;
        MuscleGroupId = muscleGroupId;
    }

    public int Id { get; init; }

    public int MuscleGroupId { get; private set; }
    public MuscleGroup MuscleGroup { get; private set; } = null!;

    public TimeConstraint TimeConstraint { get; private set; }

    public string Name { get; private set; } = null!;
}
