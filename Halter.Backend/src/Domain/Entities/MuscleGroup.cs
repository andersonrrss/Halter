using System.Text.Json.Serialization;

namespace Halter.Domain.Entities;

public class MuscleGroup
{
    public MuscleGroup() { }

    public MuscleGroup(string name)
    {
        Name = name;
    }

    [JsonConstructor]
    public MuscleGroup(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public int Id { get; init; }

    public string Name { get; private set; } = null!;
}
