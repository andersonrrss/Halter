using GymApp.API.Extensions;
using GymApp.Application.Interfaces;
using GymApp.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymApp.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : BaseController
    {
        private readonly IUserService _userService; 

        public UserController(IUserService userService, IRoutineService routineService) =>
            _userService = userService;


        [HttpGet]
        public async Task<IActionResult> GetUserInformation()
        {
            var userId = User.GetUserId();
            var result = await _userService.GetUserInformationAsync(userId);
            return Respond(result);
        }
    }
}
