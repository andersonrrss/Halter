using Microsoft.AspNetCore.Mvc;

using Halter.Application.Interfaces;
using Halter.Application.DTOs;

namespace Halter.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : BaseController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        // Registro
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] RegisterRequest registerDTO)
        {
            var result = await _authService.TryRegisterUserAsync(registerDTO);

            return Respond(result);
        }

        // Login
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginRequest loginDTO)
        {
            var result = await _authService.TryLoginUserAsync(loginDTO);

            return Respond(result);
        }
    }
}
