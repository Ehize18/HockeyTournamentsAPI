using HockeyTournamentsAPI.Application.Interfaces;
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

        public ToursController(ITourService tourService, ITournamentService tournamentService)
        {
            _tourService = tourService;
            _tournamentService = tournamentService;
        }

        [HttpPost]
        [Authorize(Roles = "Supervisor,Administrator")]
        public async Task<ActionResult<List<Tour>>> CreateTours(Guid tournamentId, [FromBody] int teamMemberCount)
        {
            var tournament = await _tournamentService.GetTournamentWithParticipants(tournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            var tours = _tourService.CreateTours(tournament, 3, teamMemberCount);

            foreach (var tour in tours)
            {
                Console.WriteLine("---Tour---");
                foreach (var match in tour.Matches)
                {
                    Console.WriteLine("---Match---");
                    for (var i = 0; i < match.Teams[0].Members.Count; i++)
                    {
                        Console.WriteLine($"{match.Teams[0].Members[i].Participant.Id} --- {match.Teams[1].Members[i].Participant.Id}");
                    }
                }
            }

            return Ok(tours);
        }
    }
}
