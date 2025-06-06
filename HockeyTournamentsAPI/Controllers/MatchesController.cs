﻿using HockeyTournamentsAPI.Application.Contracts.Matches;
using HockeyTournamentsAPI.Application.Interfaces;
using HockeyTournamentsAPI.Application.Map;
using HockeyTournamentsAPI.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HockeyTournamentsAPI.Controllers
{
    [Authorize]
    [Route("ApiV1/Tournaments/{tournamentId:guid}/Tours/{tourId:guid}/[controller]")]
    [ApiController]
    public class MatchesController : ControllerBase
    {
        private readonly IMatchService _matchService;
        private readonly ITournamentService _tournamentService;
        private readonly ITourService _tourService;

        public MatchesController(IMatchService matchService,
            ITournamentService tournamentService,
            ITourService tourService)
        {
            _matchService = matchService;
            _tournamentService = tournamentService;
            _tourService = tourService;
        }

        /// <summary>
        /// Возвращает список матчей тура. Ответ скорее всего будет меняться, я пока не знаю что и как лучше вернуть.
        /// </summary>
        /// <param name="tournamentId"></param>
        /// <param name="tourId"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<List<MatchResponse>>> GetTourMatches(Guid tournamentId, Guid tourId)
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

            var participantsWithUsers = new List<TournamentParticipant>();

            foreach (var participant in tour.Participants)
            {
                participantsWithUsers.Add((await _tournamentService.GetParticipantById(participant.Id))!);
            }

            var matches = await _matchService.GetMatchesByTourId(tourId);

            var response = new List<MatchResponse>();

            foreach (var match in matches)
            {
                foreach (var team in match.Teams)
                {
                    foreach (var member in team.Members)
                    {
                        member.Participant = participantsWithUsers
                            .FirstOrDefault(p => p.Id == member.ParticipantId)!;
                    }
                }
                response.Add(match.ToResponse());
            }

            return Ok(response);
        }

        [HttpPost("{matchId:guid}")]
        [Authorize(Roles = "Supervisor,Administrator,Judge")]
        public async Task<ActionResult<MatchResponse>> UpdateGoals(Guid tournamentId, Guid tourId, Guid matchId, [FromBody] MatchResultRequest request)
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

            var match = await _matchService.GetMatchById(matchId);

            if (match == null)
            {
                return NotFound();
            }

            var matchTask = _matchService.SetMatchResults(match, request);

            var participantsWithUsers = new List<TournamentParticipant>();

            foreach (var participant in tour.Participants)
            {
                participantsWithUsers.Add((await _tournamentService.GetParticipantById(participant.Id))!);
            }

            var matchResult = await matchTask;

            foreach (var team in matchResult.Teams)
            {
                foreach (var member in team.Members)
                {
                    member.Participant = participantsWithUsers
                        .FirstOrDefault(p => p.Id == member.ParticipantId)!;
                }
            }

            return Ok(matchResult.ToResponse());
        }
    }
}
