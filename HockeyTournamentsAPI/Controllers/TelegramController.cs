using System.Security.Claims;
using HockeyTournamentsAPI.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/[controller]")]
    [ApiController]
    public class TelegramController : ControllerBase
    {
        private readonly IUserService _userService;

        public TelegramController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> ConnectTelegramToCurrentUser([FromQuery]long telegramId)
        {
            var email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userService
                .GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound();
            }

            user.TelegramId = telegramId;

            var updated = await _userService.UpdateUserAsync(user);

            if (updated.TelegramId == telegramId)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
