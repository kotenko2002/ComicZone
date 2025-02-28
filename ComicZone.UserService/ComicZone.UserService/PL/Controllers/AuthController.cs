using ComicZone.UserService.BLL.Services.Auth;
using ComicZone.UserService.PL.Models.Auth;
using Microsoft.AspNetCore.Mvc;

using Ardalis.Result;
using ComicZone.UserService.BLL.Services.Auth;
using ComicZone.UserService.PL.Models.Auth;
using Microsoft.AspNetCore.Mvc;

namespace ComicZone.UserService.PL.Controllers
{
    [ApiController, Route("auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            return result.IsSuccess
                ? Ok("Реєстрація пройшла успішно!")
                : BadRequest(new { result.Errors });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            return result.IsSuccess
                ? Ok(result.Value)
                : Unauthorized(new { result.Errors });
        }
    }
}