﻿using ComicZone.UserService.BLL.Services.Auth;
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
            await _authService.RegisterAsync(request);

            return Ok("Реєстрація пройшла успішно!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            string accessToken = await _authService.LoginAsync(request);

            return Ok(accessToken);
        }
    }
}
