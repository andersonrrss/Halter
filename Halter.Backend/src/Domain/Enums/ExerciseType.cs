namespace GymApp.Domain.Enums;

public enum ExerciseType
{
    Fixed,
    Range,
    Pyramid,
    UpSet, // Diminui peso, aumenta reps
    DropSet, // Aumenta Reps, diminui peso
    Multiple,
    UntilFail,
    Time
}
