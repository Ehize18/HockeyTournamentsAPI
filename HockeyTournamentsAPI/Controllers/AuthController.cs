using System.Net;
using HockeyTournamentsAPI.Application.Contracts.Auth;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private const string TOKEN_COOKIE_NAME = "HockeyToken";

        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody]RegisterRequest request)
        {
            var isRegistered = await _authService.RegisterUser(request);

            if (isRegistered)
            {
                return Ok();
            }
            return BadRequest("Ошибка регистрации.");
        }

        [Route("Login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody]LoginRequest request)
        {
            var token = await _authService.LoginUser(request);

            if (string.IsNullOrWhiteSpace(token))
            {
                return BadRequest("Ошибка аутентификации.");
            }

            var cookieOptions = new CookieOptions();
            cookieOptions.HttpOnly = true;

            HttpContext.Response.Cookies.Append(TOKEN_COOKIE_NAME, token, cookieOptions);
            return Ok(token);
        }

        [Authorize]
        [Route("Logout")]
        [HttpPost]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(TOKEN_COOKIE_NAME);
            return Ok();
        }
    }
}
