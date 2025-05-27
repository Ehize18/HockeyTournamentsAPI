using HockeyTournamentsAPI.Application.Contracts.Tours;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Application.Map;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/Tournaments/{tournamentId:guid}/[controller]")]
    [ApiController]
    public class ToursController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly ITournamentService _tournamentService;
        private readonly IUserService _userService;

        public ToursController(ITourService tourService, ITournamentService tournamentService, IUserService userService)
        {
            _tourService = tourService;
            _tournamentService = tournamentService;
            _userService = userService;
        }

        /// <summary>
        /// Создаёт туры в турнире.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "Supervisor,Administrator")]
        public async Task<ActionResult<List<TourResponse>>> CreateTours(Guid tournamentId, [FromBody] TourRequest request)
        {
            var tournament = await _tournamentService.GetTournamentWithParticipants(tournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            var referee = await _userService.GetUserByIdAsync(request.RefereeId);

            if (referee == null)
            {
                return NotFound($"Судья с id: {request.RefereeId} не найден");
            }

            if (referee.Role != Role.Judge)
            {
                return BadRequest($"Пользователь с id: {request.RefereeId} не судья");
            }

            var tours = await _tourService.CreateTours(tournament, referee, request.TourCount, request.MembersInTeams);

            if (tours.Count == 0)
            {
                return BadRequest($"Не хватает игроков, для заданных параметров. Низкое количество игроков тура. При количестве игроков в командах = {request.MembersInTeams}");
            }

            var response = tours.ToResponse();

            return Ok(response);
        }

        /// <summary>
        /// Получает список всех туров турнира.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<TourResponse>>> GetTours(Guid tournamentId)
        {
            var tournament = await _tournamentService.GetById(tournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            var tours = await _tourService.GetToursByTournamentId(tournamentId);

            var response = tours.ToResponse();

            return Ok(response);
        }

        /// <summary>
        /// Получает конкретный тур турнира по id.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="tourId"></param>
        /// <returns></returns>
        [HttpGet("{tourId:guid}")]
        [Authorize]
        public async Task<ActionResult<List<TourResponse>>> GetTours(Guid tournamentId, Guid tourId)
        {
            var tournament = await _tournamentService.GetById(tournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            var tour = await _tourService.GetTourById(tourId);

            if (tour == null)
            {
                return NotFound();
            }

            var response = tour.ToResponse();

            return Ok(response);
        }
    }
}
