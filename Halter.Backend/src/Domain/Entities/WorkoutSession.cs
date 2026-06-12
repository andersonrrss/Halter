namespace Halter.Domain.Entities;

public class WorkoutSession
{
    public WorkoutSession() {}

    public WorkoutSession(Guid workoutId, Guid userId)
    {
        WorkoutId = workoutId;
        UserId = userId;
    }
    public Guid Id { get; private set; } = Guid.NewGuid();

    public Guid? WorkoutId { get; private set; }
    public Workout? Workout { get; private set; }

    public Guid UserId { get; private set; }
    public User User { get; private set;} = null!;

    public DateTime StartedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; private set; }
    public DateTime? PausedAt { get; private set; } = null;
    public int TotalPausedSeconds { get; private set; } = 0;

    public ICollection<ExerciseEntry> ExercisesEntries { get; private set; } = [];

    public TimeSpan TotalTime => FinishedAt is not null
        ? FinishedAt.Value - StartedAt - TimeSpan.FromSeconds(TotalPausedSeconds)
        : TimeSpan.Zero;

    public void FinishSession() => FinishedAt = DateTime.UtcNow;

    public void TogglePause()
    {
        if(PausedAt is null)
        {
            PausedAt = DateTime.UtcNow;
        }
        else
        {
            TotalPausedSeconds += (int)(DateTime.UtcNow - PausedAt.Value).TotalSeconds;
            PausedAt = null;
        }
    }
}
