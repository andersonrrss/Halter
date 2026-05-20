using GymApp.API.Extensions;
using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
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
        public async Task<IActionResult> StartWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.StartSessionAsync(workoutId, userId);
            return Respond(result);
        }

        [HttpPatch("finish")]
        public async Task<IActionResult> PauseWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.FinishSessionAsync(workoutId, userId);
            return Respond(result);
        }

        [HttpPatch("pause")]
        public async Task<IActionResult> FinishWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutSessionService.PauseSessionAsync(workoutId, userId);
            return Respond(result);
        }

        [HttpPost("exercise")]
        public async Task<IActionResult> AddExerciseEntry([FromRoute] Guid workoutId, [FromBody] ExerciseEntryRequest request)
        {
            var userId = User.GetUserId();
            var result = await _exerciseEntryService.AddAsync(request, userId, workoutId);
            return Respond(result);
        }
    }
}
