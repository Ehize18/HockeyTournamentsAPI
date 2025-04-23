using System.Security.Claims;
using HockeyTournamentsAPI.Application.Contracts.Participants;
using HockeyTournamentsAPI.Application.Contracts.Tournaments;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Application.Map;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/[controller]")]
    [ApiController]
    public class TournamentsController : ControllerBase
    {
        private readonly ITournamentService _tournamentService;
        private readonly IUserService _userService;

        public TournamentsController(ITournamentService tournamentService, IUserService userService)
        {
            _tournamentService = tournamentService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor,Administrator")]
        public async Task<ActionResult<TournamentResponse>> CreateAsync([FromBody]TournamentRequest request)
        {
            var tournament = await _tournamentService
                .CreateAsync(request);

            if (tournament == null)
            {
                return BadRequest();
            }

            var response = tournament.ToResponse();

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<TournamentResponse>>> GetAllAsync()
        {
            var tournamentsEntities = await _tournamentService
                .GetAllAsync();

            var response = tournamentsEntities
                .ToResponse();

            return Ok(response);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TournamentResponse>> GetById(Guid id)
        {
            var tournament = await _tournamentService
                .GetById(id);

            if (tournament == null)
            {
                return NotFound();
            }

            var response = tournament
                .ToResponse();

            return Ok(response);
        }

        [HttpPost("Start/{id:guid}")]
        public async Task<ActionResult> Start(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
