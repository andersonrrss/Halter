using Halter.Domain.Enums;

namespace Halter.Domain.Entities;

public class ExerciseEntry
{
    public ExerciseEntry() {}

    public ExerciseEntry(
        int exerciseId,
        Guid workoutSessionId,
        int valueAchieved,
        int setsCompleted,
        ExerciseType exerciseType,
        double? maxWeigth
    )
    {
        ExerciseId = exerciseId;
        WorkoutSessionId = workoutSessionId;
        ValueAchieved = valueAchieved;
        SetsCompleted = setsCompleted;
        ExerciseType = exerciseType;
        MaxWeight = maxWeigth;
    }

    public Guid Id { get; private set; } = Guid.NewGuid();

    public int ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = null!; // Deve referenciar Exercise

    public Guid WorkoutSessionId { get; private set; }
    public WorkoutSession WorkoutSession { get; private set; } = null!;

    public ExerciseType ExerciseType { get; private set; }

    public DateTime CompletedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Reps ou segundos da série principal do exercício
    /// Série principal: O pico de performance da série.
    /// DropSet -> Sub-série de maior carga(a primeira) | UpSet -> Sub-série de maior carga(a ultima)
    /// Pyramid -> Série de maior carga
    /// Time -> Segundos completados |  UntilFail -> O total de reps ou a maior carga
    /// </summary>
    public int ValueAchieved { get; private set; }

    public int SetsCompleted { get; private set; }

    public double? MaxWeight { get; private set; }
}
