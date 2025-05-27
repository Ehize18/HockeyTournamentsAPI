using System.Security.Claims;
using HockeyTournamentsAPI.Application.Contracts.Roles;
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

        /// <summary>
        /// Возвращает информацию о текущем пользователе.
        /// </summary>
        /// <returns></returns>
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
            var response = user.ToResponse();
            return Ok(response);
        }

        /// <summary>
        /// Возвращает информацию о пользователе по id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            var response = user.ToResponse();
            return Ok(response);
        }

        /// <summary>
        /// Возвращает список всех пользователей с фильтрацией и сортировкой (можно не юзать фильтры, тогда будут все)
        /// </summary>
        /// <param name="ageFrom">Возраст от</param>
        /// <param name="ageTo">Возраст до</param>
        /// <param name="page">Номер страницы</param>
        /// <param name="pageSize">Размер страниц</param>
        /// <param name="gender">Пол, true - мужчина, false - женщина</param>
        /// <param name="orderBy">Колонка для сортировки</param>
        /// <param name="isAscending">true - по возрастанию, false - по убыванию</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAll(int ageFrom = 0, int ageTo = 200, int page = 1, int pageSize = int.MaxValue, bool? gender = null, string orderBy = "Rating", bool isAscending = true)
        {
            var users = new List<User>();

            try
            {
                users = await _userService.GetUsersAsync(ageFrom, ageTo, page, pageSize, gender, orderBy, isAscending);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }

            var responses = new List<UserResponse>();

            foreach (var user in users)
            {
                responses.Add(user.ToResponse());
            }

            return Ok(responses);
        }

        /// <summary>
        /// Меняет роль пользователя.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Supervisor,Administator")]
        [HttpPatch("{id:guid}/Role")]
        public async Task<IActionResult> ChangeRole(Guid id, [FromBody] ChangeRoleRequest request)
        {
            if (!Enum.IsDefined(typeof(Role), request.RoleId))
            {
                return NotFound();
            }

            var role = (Role)request.RoleId;

            if (role == Role.Supervisor)
            {
                return BadRequest("Нельзя установить роль супер пользователя");
            }

            var isChanged = await _userService.ChangeRoleAsync(id, role);

            if (!isChanged)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
