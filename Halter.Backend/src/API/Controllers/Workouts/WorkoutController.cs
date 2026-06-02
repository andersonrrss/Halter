using GymApp.API.Extensions;
using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
{
    [Route("api/workouts")]
    [ApiController]
    [Authorize]
    public class WorkoutController : BaseController
    {
        private readonly IWorkoutService _workoutService;

        public WorkoutController(IWorkoutService workoutService) =>
            _workoutService = workoutService;

        [HttpGet("{workoutId}")]
        public async Task<ActionResult<WorkoutWithExercisesResponse>> ViewWorkout([FromRoute] Guid workoutId)
        {
            var userId = User.GetUserId();
            var result = await _workoutService.GetWorkoutAsync(workoutId, userId);
            
            return Respond(result);
        }

        [HttpPost]
        public async Task<ActionResult<WorkoutResponse>> CreateWorkout([FromBody] WorkoutRequest workoutDto)
        {
            var userId = User.GetUserId();
            var result = await _workoutService.CreateWorkoutAsync(workoutDto, userId);
            
            return Respond(result, true);
        }
    }
}
