using HockeyTournamentsAPI.Application.Contracts.Roles;
using HockeyTournamentsAPI.Application.Map;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        public RolesController() { }

        [HttpGet]
        public ActionResult<List<RoleResponse>> GetRoles()
        {
            var roles = new List<RoleResponse>();

            foreach (Role role in Enum.GetValues(typeof(Role)))
            {
                roles.Add(role.ToResponse());
            }

            return Ok(roles);
        }

        [HttpGet("{id:int}")]
        public ActionResult<RoleResponse> GetRole(int id)
        {
            if (!Enum.IsDefined(typeof(Role), id))
            {
                return NotFound();
            }

            var role = (Role)id;

            var tupe = role.GetType();

            return Ok(role.ToResponse());
        }
    }
}
