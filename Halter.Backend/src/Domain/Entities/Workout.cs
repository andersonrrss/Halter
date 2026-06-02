namespace GymApp.Domain.Entities;

public class Workout
{
    public Workout() {}

    public Workout(string name, Guid routineId)
    {
        Name = name;
        RoutineId = routineId;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public string Name { get; private set; } = null!;

    public Guid RoutineId { get; private set; }
    public Routine Routine { get; private set; } = null!;

    public ICollection<WorkoutExercise> Exercises { get; private set; } = [];
}
