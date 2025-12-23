using Books_2.Contracts.Auth;
using Books_2.Services;
using Microsoft.AspNetCore.Mvc;

namespace Books_2.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;

        public AuthController(AuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Username and password must be provided");

            var token = _authService.Login(request.Username, request.Password);
            if (token == null)
                return Unauthorized("Неверное имя пользователя или пароль");

            return Ok(new LoginResponse(token));
        }
    }
}
