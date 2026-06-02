using GymApp.API.Extensions;
using GymApp.Application.DTOs;
using GymApp.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
{
    [Route("api/routines")]
    [ApiController]
    [Authorize]
    public class RoutineController : BaseController
    {
        private readonly IRoutineService _routineService;

        public RoutineController(IRoutineService routineService)
        {
            _routineService = routineService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoutineResponse>>> GetUserRoutines()
        {
            var userId = User.GetUserId();
            var result = await _routineService.GetUserRoutinesAsync(userId);
            return Respond(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoutineResponse>> GetRoutine([FromRoute] Guid id)
        {
            var userId = User.GetUserId();
            var result = await _routineService.GetRoutineAsync(id, userId);
            return Respond(result);
        }

        [HttpPost]
        public async Task<ActionResult<RoutineResponse>> CreateRoutine([FromBody] RoutineRequest routineDTO)
        {
            var userId = User.GetUserId();
            var result = await _routineService.CreateRoutineAsync(routineDTO, userId);
            return Respond(result, true);
        }
    }
}
