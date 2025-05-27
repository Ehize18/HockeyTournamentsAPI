using System.Security.Claims;
using HockeyTournamentsAPI.Application.Contracts.Participants;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Application.Map;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Route("ApiV1/Tournaments/{tournamentId:guid}/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITournamentService _tournamentService;

        public ParticipantsController(IUserService userService, ITournamentService tournamentService)
        {
            _userService = userService;
            _tournamentService = tournamentService;
        }

        /// <summary>
        /// Записывает текущего пользователя на выбранный турнир.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<ActionResult<TournamentParticipantResponse>> AddParticipant(Guid tournamentId)
        {
            var email = User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Email)!.Value;
            var user = await _userService
                .GetUserByEmailAsync(email);

            if (user == null)
            {
                return NotFound($"Пользователь с почтой: {email} не найден.");
            }

            var tournament = await _tournamentService.GetById(tournamentId);

            if (tournament == null)
            {
                return NotFound($"Турнир с id: {tournamentId} не найден.");
            }

            if (!tournament.CanParticipate)
            {
                return BadRequest($"На турнир закрыта запись.");
            }

            var participant = await _tournamentService.AddParticipantAsync(tournament, user);

            if (participant == null)
            {
                return BadRequest("Не удалось зарегистрироваться на турнир.");
            }

            var response = participant.ToResponse(tournament, user);

            return Ok(response);
        }

        /// <summary>
        /// Получает список участников турнира.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<TournamentParticipantResponse>>> GetParticipants(Guid tournamentId)
        {
            var tournament = await _tournamentService.GetById(tournamentId);

            if (tournament == null)
            {
                return NotFound();
            }

            var participants = await _tournamentService.GetParticipantsAsync(tournamentId);

            var response = participants.ToResponse(tournament);

            response.Sort((a, b) => b.RatingOnTournament - a.RatingOnTournament);

            return response;
        }

        /// <summary>
        /// Меняет статус участия игрока в турнире.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="participantId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize(Roles = "Administrator,Supervisor")]
        [HttpPatch("{participantId:guid}")]
        public async Task<ActionResult<TournamentParticipantResponse>> AcceptParticipant(
            Guid tournamentId, Guid participantId, [FromBody] ChangeParticipantStatusRequest request)
        {
            var tournament = await _tournamentService.GetById(tournamentId);

            if (tournament == null)
            {
                return NotFound($"Турнир с id: {tournamentId} не найден.");
            }

            var participant = await _tournamentService.GetParticipantById(participantId);

            if (participant == null)
            {
                return NotFound($"Участник турнира с id: {participantId} не найден");
            }

            var changed = await _tournamentService.ChangeParticipantStatus(participant, request.IsAccepted);

            var response = changed.ToResponse(tournament);

            return Ok(response);
        }
    }
}
