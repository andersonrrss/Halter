using GymApp.API.Extensions;
using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
{
    [Route("api/workouts/{workoutId}/exercises")]
    [ApiController]
    [Authorize]
    public class WorkoutExerciseController : BaseController
    {
        private readonly IWorkoutExerciseService _workoutExerciseService;

        public WorkoutExerciseController(IWorkoutExerciseService workoutExerciseService) => 
            _workoutExerciseService = workoutExerciseService;
        
        [HttpPost]
        public async Task<ActionResult<WorkoutExerciseResponse>> AddWorkoutExercise(
            [FromRoute] Guid workoutId, 
            [FromBody] WorkoutExerciseRequest workoutExercise)
        {
            var userId = User.GetUserId();
            var result = await _workoutExerciseService.CreateWorkoutExerciseAsync(workoutId ,workoutExercise, userId);
            return Respond(result, true);
        }
    }
}
