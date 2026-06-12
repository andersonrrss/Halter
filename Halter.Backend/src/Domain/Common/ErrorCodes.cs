using System.Reflection;

namespace Halter.Domain.Common;

public static class ErrorCodes
{
    public const string Invalid = "INVALID";
    public const string Required = "REQUIRED";
    public const string AlreadyExists = "ALREADY_EXISTS";
    public const string TooShort = "TOO_SHORT";
    public const string TooLong = "TOO_LONG";
    public const string OutOfRange = "OUT_OF_RANGE";
    public const string InvalidState = "INVALID_STATE";
    public const string Empty = "EMPTY";

    public const string SessionAlreadyFinished = "SESSION_ALREADY_FINISHED";
    public const string SessionAlreadyActive = "SESSION_ALREADY_ACTIVE";
    public const string NoActiveSession = "NO_ACTIVE_SESSION";

    public const string IncompatibleExerciseType = "INCOMPATIBLE_EXERCISE_TYPE";
    public const string InvalidValuesForExerciseType = "INVALID_VALUES_FOR_EXERCISE_TYPE";

    public const string NotFound = "NOT_FOUND";

    public const string Forbidden = "FORBIDDEN";

    public const string InternalError = "INTERNAL_ERROR";

    private static readonly HashSet<string> _validCodes = typeof(ErrorCodes)
        .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
        .Where(f => f.IsLiteral && f.FieldType == typeof(string))
        .Select(f => (string)f.GetRawConstantValue()!)
        .ToHashSet();

    public static bool IsValid(string code) => _validCodes.Contains(code);
}