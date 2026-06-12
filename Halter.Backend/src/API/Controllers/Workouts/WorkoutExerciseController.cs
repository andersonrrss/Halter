using Halter.API.Extensions;
using Halter.Application.DTOs;
using Halter.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Halter.API.Controllers
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
