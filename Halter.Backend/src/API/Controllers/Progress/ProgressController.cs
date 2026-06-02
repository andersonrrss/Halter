using GymApp.API.Controllers;
using GymApp.API.Extensions;
using GymApp.Application;
using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MyApp.Namespace
{
    [Route("api/progress")]
    [ApiController]
    [Authorize]
    public class ProgressController : BaseController
    {
        private readonly IWorkoutSessionService _workoutSessionService;
        private readonly IExerciseEntryService _exerciseEntryService;

        public ProgressController
        (
            IWorkoutSessionService workoutSessionService,
            IExerciseEntryService exerciseEntryService
        )
        {
            _workoutSessionService =  workoutSessionService;
            _exerciseEntryService = exerciseEntryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinishedWorkoutSessionResponse>>> GetAllSessions(
            [FromQuery] int page = 0,
            [FromQuery] int pageSize = 20,
            [FromQuery] Guid? workoutId = null
        )
        {
            var userId = User.GetUserId();
            return Respond(
                await _workoutSessionService.GetAllByUserAsync(userId, page, pageSize, workoutId)
            );
        }

        [HttpGet("{workoutSessionId}")]
        public async Task<ActionResult<FinishedWorkoutSessionResponse>> GetSession([FromRoute] Guid workoutSessionId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.GetAsync(workoutSessionId, userId);
            return Respond(result);
        }

        [HttpGet("exercises")]
        public async Task<ActionResult<IEnumerable<ExerciseSimpleResponse>>> GetTrainedExercises 
        (
            [FromQuery] int pageSize = 20,
            [FromQuery] int page = 0,
            [FromQuery] int? muscleGroupId = null,
            [FromQuery] string? search = null
        )
        {
            var userId = User.GetUserId();
            var result = await _exerciseEntryService.
                GetTrainedExercisesAsync(userId, page, pageSize, muscleGroupId, search);
            return Respond(result);
        }

        [HttpGet("exercises/{exerciseId}")]
        public async Task<ActionResult<ExerciseProgressResponse>> GetExerciseProgress(
            [FromRoute] int exerciseId,
            [FromQuery] DateTime? from,
            [FromQuery] DateTime? to
        )
        {
            var userId = User.GetUserId();
            var result = await _exerciseEntryService.GetExerciseProgress(exerciseId, userId, from, to);
            return Respond(result);
        }
    }
}
