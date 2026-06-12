namespace Halter.Domain.Common;

public class Result
{
    public bool IsSuccess { get; init; } 
    public string? ErrorCode { get; init; }
    public FieldErrors? FieldErrors { get; init; }

    public Result(bool isSuccess, string? errorCode, FieldErrors? fieldErrors = null)
    {
        if (!isSuccess)
        {
            if(fieldErrors is not null && fieldErrors.Any())
            {
                ValidateFieldErrors(fieldErrors);
                FieldErrors = fieldErrors;
            }
            else
            {
                if(errorCode is null )
                    throw new ArgumentNullException(nameof(errorCode), "Resultado de falha requer um código de erro");
                if(!ErrorCodes.IsValid(errorCode))
                    throw new ArgumentException($"Código de erro inválido: {errorCode}");

                ErrorCode = errorCode;
            }
        }

        IsSuccess = isSuccess;
    }

    private void ValidateFieldErrors(FieldErrors fieldErrors)
    {
        foreach(var field in fieldErrors)
        {
            if(!ErrorCodes.IsValid(field.Value[0]))
                throw new ArgumentException(
                    $"Erro inválido: '{field.Value[0]}' no campo '{field.Key}'", 
                    nameof(fieldErrors)
                );
        }
    }

    public static Result Success() => new(true, null);

    public static Result Failure(string errorCode) => new(false, errorCode);

    public static Result Forbidden() => Failure(ErrorCodes.Forbidden);

    public static Result NotFound() => Failure(ErrorCodes.NotFound);

    public static Result InternalError() => Failure(ErrorCodes.InternalError);

    public static Result Invalid() => Failure(ErrorCodes.Invalid);
    public static Result Required() => Failure(ErrorCodes.Required);
    public static Result AlreadyExists() => Failure(ErrorCodes.AlreadyExists);
    public static Result TooShort() => Failure(ErrorCodes.TooShort);
    public static Result TooLong() => Failure(ErrorCodes.TooLong);
    public static Result OutOfRange() => Failure(ErrorCodes.OutOfRange);
    public static Result InvalidState() => Failure(ErrorCodes.InvalidState);
    public static Result Empty() => Failure(ErrorCodes.Empty);
}
