using Halter.API.Extensions;
using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Halter.API.Controllers
{
    [Route("api/workouts/{workoutId}/session")]
    [ApiController]
    [Authorize]
    public class WorkoutSessionController : BaseController
    {
        private readonly IWorkoutSessionService _workoutSessionService;
        private readonly IExerciseEntryService _exerciseEntryService;

        public WorkoutSessionController(
            IWorkoutSessionService workoutSessionService,
            IExerciseEntryService exerciseEntryService
        )
        {
            _workoutSessionService = workoutSessionService;
            _exerciseEntryService = exerciseEntryService;
        }

        [HttpPost("start")]
        public async Task<ActionResult<UnfinishedWorkoutSessionResponse>> StartWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.StartSessionAsync(workoutId, userId);
            return Respond(result);
        }

        [HttpPatch("finish")]
        public async Task<ActionResult<FinishedWorkoutSessionResponse>> FinshWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.FinishSessionAsync(workoutId, userId);
            return Respond(result);
        }

        [HttpPatch("pause")]
        public async Task<ActionResult<UnfinishedWorkoutSessionResponse>> PauseWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.PauseSessionAsync(workoutId, userId);
            return Respond(result);
        }

        [HttpPost("exercise")]
        public async Task<ActionResult<ExerciseEntryResponse>> AddExerciseEntry([FromRoute] Guid workoutId, [FromBody] ExerciseEntryRequest request)
        {
            var userId = User.GetUserId();
            var result = await _exerciseEntryService.AddAsync(request, userId, workoutId);
            return Respond(result);
        }
    }
}
