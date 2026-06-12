using Halter.API.Extensions;
using Halter.Application.Interfaces;
using Halter.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Halter.API.Controllers
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
        public async Task<ActionResult<UserResponse>> GetUserInformation()
        {
            var userId = User.GetUserId();
            var result = await _userService.GetUserInformationAsync(userId);
            return Respond(result);
        }
    }
}
