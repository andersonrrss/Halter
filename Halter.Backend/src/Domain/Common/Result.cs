using GymApp.Domain.Enums;

namespace GymApp.Domain.Common;

public class Result
{
    public bool IsSuccess { get; init; } 
    public string? Error { get; init; }
    public ErrorType ErrorType { get; init; }

    public Result(bool isSuccess,string? error, ErrorType errorType)
    {
        IsSuccess = isSuccess;
        Error = error;
        ErrorType = errorType;
    }

    public static Result Success() => new(true, null, ErrorType.None);

    public static Result Failure(string error, ErrorType errorType) => new(false, error, errorType);

    public static Result BusinessFailure(string message = "Erro de negócios") =>
        new(false, message, ErrorType.Business);

    public static Result Forbidden(string message = "Acesso negado") =>
        new(false, message, ErrorType.Forbidden);

    public static Result NotFound(string message = "Não encontrado") =>
        new(false, message, ErrorType.NotFound);

    public static Result InternalError(string message = "Erro interno") =>
        new(false, message, ErrorType.InternalError);
}
