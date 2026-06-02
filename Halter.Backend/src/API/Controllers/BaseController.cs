using GymApp.Domain.Enums;
using GymApp.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult<T> Respond<T>(Result<T> result, bool created = false)
        {
            if (!result.IsSuccess)
                return MapError<T>(result.ErrorType, result.Error, result.FieldErrors);

            return created ? Created(string.Empty, result.Value) : Ok(result.Value);
        }

        protected ActionResult Respond(Result result) => 
            result.IsSuccess ? Ok() : MapError(result.ErrorType, result.Error);
        
        private ActionResult<T> MapError<T>(
            ErrorType errorType,
            string? error,
            Dictionary<string, string[]>? fieldErrors = null
        ) => MapError(errorType, error, fieldErrors);

        private ActionResult MapError(
            ErrorType errorType,
            string? error,
            Dictionary<string, string[]>? fieldErrors = null
        ) => errorType switch
        {
            ErrorType.NotFound => NotFound(new { message = error }),

            ErrorType.InternalError => StatusCode(500, new { message = error }),

            ErrorType.Forbidden => StatusCode(403, new { message = error } ),

            ErrorType.Field => fieldErrors is not null
                ? ValidationProblem(new ValidationProblemDetails(fieldErrors))
                : StatusCode(500, new { message = "Erro interno de validação" }),

            _ => BadRequest(new { message = error })
        };
    }
}
