using Halter.Domain.Enums;
using Halter.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Halter.API.Controllers
{
    public class BaseController : ControllerBase
    {
        protected ActionResult<T> Respond<T>(Result<T> result, bool created = false)
        {
            if (!result.IsSuccess)
                return MapError<T>(result.ErrorCode!, result.FieldErrors);

            return created ? Created(string.Empty, result.Value) : Ok(result.Value);
        }

        protected ActionResult Respond(Result result) => 
            result.IsSuccess ? Ok() : MapError(result.ErrorCode!);
        
        private ActionResult<T> MapError<T>(
            string errorCode,
            FieldErrors? fieldErrors = null
        ) => MapError(errorCode, fieldErrors);

        private ActionResult MapError(
            string errorCode,
            FieldErrors? fieldErrors = null
        )
        {
            if(fieldErrors is not null && fieldErrors.Any())
                return ValidationProblem(new ValidationProblemDetails(fieldErrors));
            
            return errorCode switch {
            ErrorCodes.NotFound => NotFound(),

            ErrorCodes.InternalError => StatusCode(500, new { code = errorCode }),

            ErrorCodes.Forbidden => StatusCode(403, new { code = errorCode }),

            _ => BadRequest(new { code = errorCode })
            };
        }
    }
}
