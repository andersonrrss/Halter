using GymApp.Domain.Enums;

namespace GymApp.Domain.Common;

public class Result<T>
{
    public bool IsSuccess { get; } 
    public T? Value { get; }
    public string? Error { get; }
    public FieldErrors? FieldErrors { get; }
    public ErrorType ErrorType { get; }

    public Result(bool isSuccess, T? value, string? error, FieldErrors? fieldErrors, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Value = value;
        Error = error;
        FieldErrors = fieldErrors;
        ErrorType = errorType;
    }

    public static Result<T> Success(T value) => 
        new(true, value, null, null, ErrorType.None);
        
    public static Result<T> Failure(string error, ErrorType errorType) => 
        new(false, default, error, null, errorType);

    public static Result<T> FieldFailure(FieldErrors errors) => 
        new (false, default, null, errors, ErrorType.Field);

    public static Result<T> FieldFailure(string errorField, string errorMessage) {
        var validationError = new FieldErrors
        {
            [errorField] =  [errorMessage]
        };
        return new (false, default, null, validationError, ErrorType.Field);
    }

    public static Result<T> BusinessFailure(string message = "Erro de negócios") =>
        new(false, default, message, null, ErrorType.Business);

    public static Result<T> Forbidden(string message = "Acesso negado") =>
        new(false, default, message, null, ErrorType.Forbidden);

    public static Result<T> NotFound(string message = "Não encontrado") =>
        new(false, default, message, null, ErrorType.NotFound);

    public static Result<T> InternalError(string message = "Erro interno") =>
        new(false, default, message, null, ErrorType.InternalError);
}
