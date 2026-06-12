namespace Halter.Domain.Common;

public class Result<T> : Result
{
    public T? Value { get; init; }

    public Result(bool isSuccess, T? value, string? errorCode, FieldErrors? fieldErrors = null) : base(isSuccess, errorCode, fieldErrors)
    {
        if(isSuccess && value is null)
            throw new ArgumentNullException(nameof(value), "Resultado de sucesso requer um valor");

        Value = value;
    }

    public static Result<T> Success(T value) => new(true, value, null);

    public static new Result<T> Failure(string errorCode) => new(false, default, errorCode);

    public static Result<T> FieldFailure(string field, string errorCode)
        => new(false, default, null, new(field, errorCode));

    public static Result<T> FieldFailure(FieldErrors fieldErrors) 
        => new(false, default, null, fieldErrors);

    public static new Result<T> Forbidden() => Failure(ErrorCodes.Forbidden);

    public static new Result<T> NotFound() => Failure(ErrorCodes.NotFound);

    public static new Result<T> InternalError() => Failure(ErrorCodes.InternalError);

    public static new Result<T> Invalid() => Failure(ErrorCodes.Invalid);
    public static Result<T> Invalid(string field) => FieldFailure(field, ErrorCodes.Invalid);

    public static new Result<T> Required() => Failure(ErrorCodes.Required);
    public static Result<T> Required(string field) => FieldFailure(field, ErrorCodes.Required);

    public static new Result<T> AlreadyExists() => Failure(ErrorCodes.AlreadyExists);
    public static Result<T> AlreadyExists(string field) => FieldFailure(field, ErrorCodes.AlreadyExists);

    public static new Result<T> TooShort() => Failure(ErrorCodes.TooShort);
    public static Result<T> TooShort(string field) => FieldFailure(field, ErrorCodes.TooShort);

    public static new Result<T> TooLong() => Failure(ErrorCodes.TooLong);
    public static Result<T> TooLong(string field) => FieldFailure(field, ErrorCodes.TooLong);

    public static new Result<T> OutOfRange() => Failure(ErrorCodes.OutOfRange);
    public static Result<T> OutOfRange(string field) => FieldFailure(field, ErrorCodes.OutOfRange);

    public static new Result<T> InvalidState() => Failure(ErrorCodes.InvalidState);
    public static Result<T> InvalidState(string field) => FieldFailure(field, ErrorCodes.InvalidState);

    public static new Result<T> Empty() => Failure(ErrorCodes.Empty);
    public static Result<T> Empty(string field) => FieldFailure(field, ErrorCodes.Empty);
}