using GymApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
{   
    [Route("api/exercises")]
    [ApiController]
    [Authorize]
    public class ExerciseController : BaseController
    {
        private readonly IExerciseService _exerciseService;

        public ExerciseController(IExerciseService exerciseService)
        {
            _exerciseService = exerciseService;
        }

        [HttpGet]
        public async Task<IActionResult> GetExercisesAsync(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 20,
            [FromQuery] int? muscleGroupId = null,
            [FromQuery] string? search = null
        )
        {
            var result = await _exerciseService.GetExercisesAsync(page, pageSize, muscleGroupId, search);
            return Respond(result);
        }
    }
}
