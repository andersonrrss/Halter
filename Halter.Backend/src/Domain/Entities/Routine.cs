namespace GymApp.Domain.Entities;

public class Routine
{
    public Routine () { }

    public Routine (string name, Guid userId)
    {
        Name = name;
        UserId = userId; 
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = null!;

    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public ICollection<Workout> Workouts { get; private set; } = [];
}
