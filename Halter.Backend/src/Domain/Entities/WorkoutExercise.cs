using Halter.Domain.Common;
using Halter.Domain.Enums;

namespace Halter.Domain.Entities;

public class WorkoutExercise
{
    public Guid Id { get; private set; } = Guid.NewGuid();

    public int ExerciseId { get; private set; }
    public Exercise Exercise { get; private set; } = null!;

    public Guid WorkoutId { get; private set; }
    public Workout Workout { get; private set; } = null!;

    public int Sets { get; private set; }
    
    public ExerciseType ExerciseType { get; private set; }
    public IList<int> Values { get; private set; } = [];
    public int? IsometricHoldSeconds { get; private set; }

    public WorkoutExercise() {}

    private WorkoutExercise (
        int exerciseId, 
        Guid workoutId,
        int sets,
        IList<int> values,
        ExerciseType exerciseType,
        int? isometricHoldSeconds
    )
    {
        ExerciseId = exerciseId;
        WorkoutId = workoutId;
        Sets = sets;
        Values = values;
        ExerciseType = exerciseType;
        IsometricHoldSeconds = isometricHoldSeconds;
    }

    public static Result<WorkoutExercise> Create(
        int exerciseId,
        Guid workoutId,
        int sets,
        IList<int> values,
        ExerciseType exerciseType = ExerciseType.Fixed,
        TimeConstraint timeConstraint = TimeConstraint.Optional,
        int? isometricHoldSeconds = null
    )
    {
        if(
            timeConstraint == TimeConstraint.Required && exerciseType != ExerciseType.Time ||
            timeConstraint == TimeConstraint.Forbidden && exerciseType == ExerciseType.Time
        )
            return Result<WorkoutExercise>.FieldFailure("ExerciseType", ErrorCodes.IncompatibleExerciseType);

        if(values.Count > 10)
            return Result<WorkoutExercise>.OutOfRange("Values");

        if(sets is > 20 or < 1)
            return Result<WorkoutExercise>.OutOfRange("Sets");

        if(isometricHoldSeconds is not null and > 300 or < 0)
            return Result<WorkoutExercise>.OutOfRange("IsometricHoldSeconds");

        var result = ValidateExercise(exerciseType, values, sets);
        if(result is not null )
            return result;

        return Result<WorkoutExercise>.Success( new WorkoutExercise(
            exerciseId, workoutId, sets, values, exerciseType, isometricHoldSeconds
        ));
    }

    private static Result<WorkoutExercise>? ValidateExercise(ExerciseType exerciseType, IList<int> values, int sets)
    {
        return exerciseType switch
        {
            ExerciseType.UntilFail => isValidUntilFail(values) ? null 
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),
                
            ExerciseType.Fixed => isValidFixed(values) ? null 
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),
                
            ExerciseType.UpSet => isValidUpSet(values) ? null
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),

            ExerciseType.DropSet => isValidDropSet(values) ? null
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),

            ExerciseType.Time => isValidTime(values) ? null
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),

            ExerciseType.Range => isValidRange(values) ? null
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),

            ExerciseType.Pyramid => isValidPyramid(values, sets) ? null
                : Result<WorkoutExercise>.FieldFailure("Values", ErrorCodes.InvalidValuesForExerciseType),

            // ExerciseType.Multiple => Result<WorkoutExercise>.FieldFailure("ExerciseType", "NO_RELEASED"),

            _ => Result<WorkoutExercise>.Invalid("ExerciseType")
        }; 
    }

    private static bool isValidUntilFail(IList<int> values) => 
        values.Count == 0 || 
        values.All(v => v == 0); // tecnicamente se todos os values forem 0 então não tem nada...
        
    private static bool isValidFixed(IList<int> values) => 
        values.Count == 1 && 
        IsValidValueRange(values);

    private static bool isValidUpSet(IList<int> values) => 
        values.Count >= 2 &&
        values.Zip(values.Skip(1)).All(p => p.First > p.Second) && 
        IsValidValueRange(values);

    private static bool isValidDropSet(IList<int> values) => 
        values.Count >= 2 &&
        values.Zip(values.Skip(1)).All(p => p.First < p.Second) && 
        IsValidValueRange(values);

    private static bool isValidTime(IList<int> values) => 
        values.Count == 1 
        && values[0] is > 0 and < 3600; // Adiciona um limite de 1h 

    private static bool isValidRange(IList<int> values) => 
        values.Count == 2 && 
        values[0] < values[1] &&
        IsValidValueRange(values);

    private static bool isValidPyramid(IList<int> values, int sets)
    {
        if(values.Count != sets)
            return false;

        if(!IsValidValueRange(values))
            return false;

        var zippedValues = values.Zip(values.Skip(1));

        return zippedValues.All(p => p.First > p.Second) || // Pirâmide em ordem decrescente
            zippedValues.All(p => p.First < p.Second) ; // Pirâmide em ordem crescente
    }

    // Define um limite de 50 reps para cada value
    private static bool IsValidValueRange(IList<int> values) => values.All(v => v is > 0 and < 51);
}

