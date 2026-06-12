namespace Halter.Application.DTOs;

public record class ExerciseEntryRequest
{
    public Guid WorkoutExerciseId { get; init; }
    public int ValueAchieved { get; init; }
    public int SetsCompleted { get; init; }
    public  double? MaxWeight { get; init; }
}