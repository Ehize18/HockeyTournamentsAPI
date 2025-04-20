using System.Security.Claims;
using HockeyTournamentsAPI.Application.Contracts.Users;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Application.Map;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [Authorize]
        [HttpGet("Me")]
        public async Task<ActionResult<UserResponse>> GetMe()
        {
            var email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userService
                .GetUserByEmailAsync(email);
            
            if (user == null)
            {
                return NotFound();
            }
            var response = user.MapToResponse();
            return Ok(response);
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<UserResponse>> GetById(Guid id)
        {
            var user = await _userService
                .GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            var response = user.MapToResponse();
            return Ok(response);
        }

        [Authorize(Roles = "Supervisor,Administator")]
        [HttpPatch("{id:guid}/Role")]
        public async Task<IActionResult> ChangeRole(Guid id, [FromBody]int roleId)
        {
            var isChanged = await _userService.ChangeRoleAsync(id, (Role)roleId);

            if (!isChanged)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
